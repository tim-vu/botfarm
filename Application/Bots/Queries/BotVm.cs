using System;
using AutoMapper;
using FORFarm.Application.Common.Mapping;
using FORFarm.Domain.Entities;

namespace FORFarm.Application.Bots.Queries
{
    public class BotVm : IMapFrom<Bot>
    {
        public int AccountId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime LastUpdate { get; set; }
        
        public DateTime LastMule { get; set; }

        public int GoldEarned { get; set; }
    }
}