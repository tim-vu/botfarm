using System;

namespace FORFarm.Domain.Entities
{
    public class Mule : Instance
    {
        public int World { get; set; }

        public int Gold { get; set; }
        
        public Position Position { get; set; }
    }
}
