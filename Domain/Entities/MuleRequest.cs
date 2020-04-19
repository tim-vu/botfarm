namespace FORFarm.Domain.Entities
{
    public class MuleRequest
    {
        public int MuleId { get; set; }
        
        public Mule Mule { get; set; }

        public int BotId { get; set; }
        
        public Bot Bot { get; set; }
    }
}