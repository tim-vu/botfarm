namespace FORFarm.Application.Common.Models.Farm.Bot
{
    public class BotResponse
    {
        public Command Command { get; }

        public BotResponse(Command command)
        {
            Command = command;
        }
    }
}