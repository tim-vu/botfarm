using FORFarm.Domain.Entities;

namespace FORFarm.Application.Bots.Commands.RequestMuling
{
    public class RequestMulingResponse
    {
        public bool Accepted { get; }

        public MuleInfo MuleInfo { get; }

        public RequestMulingResponse(MuleInfo muleInfo)
        {
            Accepted = true;
            MuleInfo = muleInfo;
        }

        public RequestMulingResponse()
        {
            
        }
    }

    public class MuleInfo
    {
        public string DisplayName { get; }
        
        public Position Position { get; }
        
        public int World { get; }

        public MuleInfo(string displayName, Position position, int world)
        {
            DisplayName = displayName;
            Position = position;
            World = world;
        }
    }
}