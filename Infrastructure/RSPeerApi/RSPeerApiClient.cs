using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using FORFarm.Application.Common.Interfaces;
using FORFarm.Application.Common.Models.Farm;
using Infrastructure.RSPeerApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Infrastructure.RSPeerApi
{
    public class RsPeerApiClient : IClientHandler
    {
        private readonly HttpClient _httpClient;

        public RsPeerApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        private const string ConnectedLaunchersUrl = "https://services.rspeer.org/api/botLauncher/connected";

        public async Task<IEnumerable<ILauncher>> GetLaunchers(string apiKey)
        {
            var request = CreateRequest(apiKey);
            request.Method = HttpMethod.Get;
            request.RequestUri = new Uri(ConnectedLaunchersUrl);

            var httpResponse = await _httpClient.SendAsync(request);

            if (!httpResponse.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await httpResponse.Content.ReadAsStringAsync();

            var response = JObject.Parse(content);

            var launchers = new List<Launcher>();

            foreach(var pair in response)
            {
                var socketAddress = Guid.Parse(pair.Key);
                var launcher = pair.Value.ToObject<Launcher>();
                launcher.SocketAddress = socketAddress;
                launchers.Add(launcher);
            }

            return launchers;
        }

        private const string ConnectedClientsUrl = "https://services.rspeer.org/api/botLauncher/connected";

        public async Task<IEnumerable<IClientInstance>> GetLaunchedClients(string apiKey)
        {
            var request = CreateRequest(apiKey);

            request.Method = HttpMethod.Get;
            request.RequestUri = new Uri(ConnectedClientsUrl);

            var httpResponse = await _httpClient.SendAsync(request);

            if (!httpResponse.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await httpResponse.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<ClientInstance>>(content);
        }

        private const string SendCommandUrl = "https://services.rspeer.org/api/botLauncher/send";

        public async Task<bool> StartClient(Guid socketAddress, ClientStartArgs clientStartArgs, string apiKey)
        {
            var client = new Client
            {
                ScriptName = clientStartArgs.ScriptName,
                ScriptArgs = clientStartArgs.ScriptArgs,
                IsRepoScript = false,
                Username = clientStartArgs.Username,
                Password = clientStartArgs.Password
            };

            if (clientStartArgs.Proxy != null)
            {        
                client.UseProxy = true;
                client.ProxyIp = clientStartArgs.Proxy.Ip;
                client.ProxyPort = clientStartArgs.Proxy.Port;
                client.ProxyUser = clientStartArgs.Proxy.Username;
                client.ProxyPass = clientStartArgs.Proxy.Password;
            }
            
            var startClientPayload = new Payload
            {
                Type = "start:client",
                Session = apiKey,
                QuickStart = new QuickStart(client)
            };

            var startClient = new StartClient(socketAddress, startClientPayload);
            
            var content = new StringContent(JsonConvert.SerializeObject(startClient), Encoding.UTF8, MediaTypeNames.Application.Json);

            var request = CreateRequest(apiKey);
            request.Method = HttpMethod.Post;
            request.RequestUri = new Uri(SendCommandUrl);
            request.Content = content;

            var httpResponse = await _httpClient.SendAsync(request);

            return httpResponse.IsSuccessStatusCode;
        }

        private const string KillClientUrl = "https://services.rspeer.org/api/botLauncher/sendNew?message=:kill&tag=";

        public async Task<bool> KillClient(Guid clientTag, string apiKey)
        {
            var content = new StringContent("");

            var request = CreateRequest(apiKey);
            request.Method = HttpMethod.Post;
            request.RequestUri = new Uri(KillClientUrl + clientTag);
            request.Content = content;

            var httpResponse = await _httpClient.SendAsync(request);

            return httpResponse.IsSuccessStatusCode;
        }
        
        private HttpRequestMessage CreateRequest(string apiKey)
        {
            return new HttpRequestMessage
            {
                Headers = { {"ApiClient", apiKey }}
            };
        }
    }
}
