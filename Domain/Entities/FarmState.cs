using System;

namespace FORFarm.Domain.Entities
{
    public class FarmState
    {
        public int ID { get; set; } = 1;
        
        public bool Running { get; set; }

        public Guid SocketAddress { get; set; }

        public DateTime Start { get; set; }
    }
}