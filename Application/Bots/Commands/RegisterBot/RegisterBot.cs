using System;
using FORFarm.Application.Common.Models.Farm.Bot;
using MediatR;

namespace FORFarm.Application.Bots.Commands.RegisterBot
{
    public class RegisterBot : IRequest<BotResponse>
    {
        public string Username { get; set; }
        
        public Guid Tag { get; set; }
    }
}