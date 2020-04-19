using System;
using System.Collections.Generic;
using System.Text;

namespace FORFarm.Domain.Entities
{
    public class FarmSettings
    {
        public int ID { get; set; } = 1;
        
        public int LaunchSleep { get; set; } = 5000;

        public bool UseProxies { get; set; } = true;

        public int MaxActiveBots { get; set; } = 10;

        public int MinActiveMules { get; set; } = 1;

        public int MaxActiveMules { get; set; } = 3;

        public int ConcurrentAccountsPerProxy { get; set; } = 3;

        public int MaxAccountsPerProxy { get; set; } = 10;

        public string Hostname { get; set; }
        
        public string ApiKey { get; set; }
        
        public string BotScriptName { get; set; }
        
        public string MuleScriptName { get; set; }

        public TimeSpan MuleInterval { get; set; } = TimeSpan.FromMinutes(30);
    }
}
