using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bogus;
using FORFarm.Domain.Entities;
using FORFarm.Domain.Enums;
using FORFarm.Domain.ValueObjects;

namespace Application.UnitTests.Common
{
    public class Data
    {
        public static IReadOnlyList<Proxy> Proxies => new List<Proxy>()
        {
            new Proxy {Ip = "162.63.86.92", Port = 11256},
            new Proxy {Ip = "52.42.112.241", Port = 40144},
            new Proxy {Ip = "254.237.134.225", Port = 32535},
            new Proxy {Ip = "72.108.61.85", Port = 27195},
            new Proxy {Ip = "177.133.169.245", Port = 47431}
        }.AsReadOnly();

        public static readonly Position Position = new Position
        {
            X = 1,
            Y = 1,
            Z = 1
        };
        
        public static readonly FarmSettings Settings = new FarmSettings
        {
            LaunchSleep = 10000,
            UseProxies = true,
            BotScriptName = "ForFarm",
            MuleScriptName = "ForFarm_Mule",
            MinActiveMules = 3,
            MaxActiveMules = 5,
            MaxActiveBots = 20,
            ApiKey = "1O7F660L7FXBB0X8KS6BXB5BOH3XWHWIRFQ7Q3ASQRPF19F2E9CYE020T51YDDYZTOT6KZ",
            Hostname = "MyHostname"
        };
    }
}
