using System;
using FORFarm.Application.Common.Models.Farm.Bot;
using FORFarm.Application.Common.Models.Farm.Mule;
using MediatR;

namespace FORFarm.Application.Mules.Commands.RegisterMule
{
    public class RegisterMule : IRequest<MuleResponse>
    {
        public string Username { get; set; }
        
        public string DisplayName { get; set; }
        
        public Guid Tag { get; set; }
    }
}