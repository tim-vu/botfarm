using System;
using FORFarm.Application.Common.Interfaces;

namespace Infrastructure.RSPeerApi.Models
{
    public class ClientInstance : IClientInstance
    {
        public DateTime LastUpdate { get; set; }

        public string ProxyIp { get; set; }

        public string MachineName { get; set; }

        public string ScriptName { get; set; }
        
        public string Rsn { get; set; }

        public string Username { get; set; }
        public Guid Tag { get; set; }
        
        public int Game { get; set; }

        public ClientInstance(DateTime lastUpdate, string proxyIp, string machineName, string scriptName, string rsn,
            string username, Guid tag, int game)
        {
            LastUpdate = lastUpdate;
            ProxyIp = proxyIp;
            MachineName = machineName;
            ScriptName = scriptName;
            Rsn = rsn;
            Username = username;
            Tag = tag;
            Game = game;
        }
    }
}
