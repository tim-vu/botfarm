using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using FORFarm.Application.Common.Mapping;
using FORFarm.Domain.Entities;

namespace FORFarm.Application.Settings
{
    public class SettingsVm : IMapFrom<FarmSettings>
    {
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

        public int MuleIntervalMinutes { get; set; } = 30;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<FarmSettings, SettingsVm>()
                .ForMember(s => s.MuleIntervalMinutes, opt => opt.MapFrom(s => s.MuleInterval.TotalMinutes));
        }
    }
}
