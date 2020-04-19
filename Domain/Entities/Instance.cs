using System;

namespace FORFarm.Domain.Entities
{
    public abstract class Instance
    {
        public int ID { get; set; }
        
        public int AccountId { get; set; }
        
        public Account Account { get; set; }
        
        public string DisplayName { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime LastUpdate { get; set; }

        public Guid Tag { get; set; }

    }
}