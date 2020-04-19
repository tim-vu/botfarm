using System;
using FORFarm.Application.Common.Models.Farm.Bot;
using MediatR;

namespace FORFarm.Application.Bots.Commands.SignalHeartbeat
{
    public class SignalBotHeartbeat : IRequest<BotResponse>
    {
        public Guid Tag { get; set; }
    }
}