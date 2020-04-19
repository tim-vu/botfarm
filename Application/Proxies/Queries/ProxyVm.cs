using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using FORFarm.Application.Common.Mapping;
using FORFarm.Domain.Entities;

namespace FORFarm.Application.Proxies.Queries
{
    public class ProxyVm : IMapFrom<Proxy>
    {
        public int ID { get; set; }

        public string Ip { get; set; }

        public int Port { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
        
        public int CurrentAccounts { get; set; }
        
        public int PreviousAccounts { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Proxy, ProxyVm>()
                .ForMember(d => d.CurrentAccounts, opt => opt.MapFrom(p => p.ActiveAccounts.Count))
                .ForMember(d => d.PreviousAccounts, opt => opt.MapFrom(p => p.BannedBots + p.BannedMules));
        }
    }
}
