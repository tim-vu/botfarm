using FORFarm.Application.Proxies.Queries;

namespace FORFarm.Application.Common.Models.Farm
{
    public class ClientStartArgs
    {
        public string Username { get; }

        public string Password { get; }

        public string ScriptName { get; }

        public string ScriptArgs { get; set; }

        public ProxyVm Proxy { get; set; }

        public ClientStartArgs(string username, string password, string scriptName)
        {
            Username = username;
            Password = password;
            ScriptName = scriptName;
        }
    }
}
