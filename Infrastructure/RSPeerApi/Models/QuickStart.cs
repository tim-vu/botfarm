using System;
using System.Collections.Generic;

namespace Infrastructure.RSPeerApi.Models
{
    [Serializable]
    public class QuickStart
    {

        public List<Client> Clients { get; }

        public QuickStart(params Client[] clients)
        {
            Clients = new List<Client>(clients);
        }
        
    }
}
