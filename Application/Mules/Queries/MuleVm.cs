using System;
using AutoMapper;
using FORFarm.Application.Bots.Queries;
using FORFarm.Application.Common.Mapping;
using FORFarm.Domain.Entities;

namespace FORFarm.Application.Mules.Queries
{
    public class MuleVm : IMapFrom<Mule>
    {
        public int AccountId { get; set; }
        
        public int? ProxyId { get; set; }
        
        public DateTime StartTime { get; set; }

        public DateTime LastUpdate { get; set; }

        public int Gold { get; set; }
        
        public int World { get; set; }
        
        public Position Position { get; set; }

        public string DisplayName { get; set; }

        public bool InGame { get; set; }
    }
}