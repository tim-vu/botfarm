using FORFarm.Application.Common.Models.Farm.Bot;
using FORFarm.Domain.Entities;

namespace FORFarm.Application.Common.Models.Farm.Mule
{
    public class MuleResponse
    {
        public Command Command { get; }
        
        public Position Position { get; }

        public MuleResponse(Command command)
        {
            Command = command;
        }

        public MuleResponse(Command command, Position position)
        {
            Command = command;
            Position = position;
        }
    }
}