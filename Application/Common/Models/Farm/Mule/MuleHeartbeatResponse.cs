using System.Collections.Generic;
using FORFarm.Application.Common.Models.Farm.Bot;

namespace FORFarm.Application.Common.Models.Farm.Mule
{
    public class MuleHeartbeatResponse
    {
        public Command Command { get; }
        
        public List<string> MuleRequests { get; }
        
        public MuleHeartbeatResponse(Command command)
        {
            Command = command;
        }

        public MuleHeartbeatResponse(Command command, List<string> muleRequests)
        {
            Command = command;
            MuleRequests = muleRequests;
        }
    }
}