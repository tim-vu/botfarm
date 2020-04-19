using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FORFarm.Application.Common.Mapping;
using FORFarm.Domain.Entities;
using FORFarm.Domain.Enums;
using FORFarm.Domain.ValueObjects;

namespace FORFarm.Application.Accounts.Queries
{
    public class AccountVm : IMapFrom<Account>
    {
        public int ID { get; set; }
    
        public string Username { get; set; }
        
        public string Password { get; set; }
        
        public bool Mule { get; set; }

        public DateTime MemberExpirationDate { get; set; }
        
        public bool Banned { get; set; }
        
        public int GoldEarned { get; set; }
        
        public int RuntimeMinutes { get; set; }
        
        public int? ProxyId { get; set; }
        
        public int TotalLevel { get; set; }

        public virtual ICollection<Skill> Skills { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Account, AccountVm>()
                .ForMember(d => d.TotalLevel, opt => opt.MapFrom(a => a.Skills.Sum(skill => skill.Level)));
        }
    }
}