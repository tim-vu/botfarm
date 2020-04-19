using System;
using FORFarm.Application.Common.Interfaces;

namespace Infrastructure.RSPeerApi.Models
{
    public class Launcher : ILauncher
    {
        public Guid SocketAddress { get; set; }
        
        public string Ip { get; }
        
        public string Hostname { get; }
        
        public string Platform { get; }
        
        public string Type { get; }
    
        public UserInfo UserInfo { get; }
        
        public Launcher(string ip, string hostname, string platform, string type, UserInfo userInfo)
        {
            Hostname = hostname;
            Platform = platform;
            Type = type;
            UserInfo = userInfo;
            Ip = ip;
        }
    }

    public class UserInfo
    {
        public string Username { get; }

        public UserInfo(string username)
        {
            Username = username;
        }
    }
}
