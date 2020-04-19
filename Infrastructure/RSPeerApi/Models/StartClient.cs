using System;
using Newtonsoft.Json;

namespace Infrastructure.RSPeerApi.Models
{
    public class StartClient
    {
        [JsonProperty(PropertyName = "socket")]
        public Guid SocketAddress { get; }

        public Payload Payload { get; }

        public StartClient(Guid socketAddress, Payload payload)
        {
            SocketAddress = socketAddress;
            Payload = payload;
        }
    }

    public class Payload
    {
        public string Type { get; set; }

        public string Session { get; set; }
        
        [JsonProperty(PropertyName = "qs")]
        public QuickStart QuickStart { get; set; }

        public string JvmArgs { get; set; } = "-Xmx384m -Djava.net.preferIPv4Stack=true -Djava.net.preferIPv4Addresses=true -Xss2m";

        public int Sleep { get; set; } = 10;

        public int Count { get; set; } = 1;

    }
}
