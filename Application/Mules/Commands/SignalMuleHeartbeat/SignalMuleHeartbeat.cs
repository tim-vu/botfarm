using System;
using FORFarm.Application.Common.Models.Farm.Bot;
using FORFarm.Application.Common.Models.Farm.Mule;
using MediatR;

namespace FORFarm.Application.Mules.Commands.SignalMuleHeartbeat
{
    public class SignalMuleHeartbeat : IRequest<MuleHeartbeatResponse>
    {
        public Guid Tag { get; set; }
        
        public int Gold { get; set; }
    }
}