using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FORFarm.Domain.Entities
{
    public class Proxy
    {
        public int ID { get; set;  }

        public string Ip { get; set; }

        public int Port { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
        
        public virtual ICollection<Account> ActiveAccounts { get; } = new HashSet<Account>();

        [NotMapped] public int PreviousAccounts => BannedBots + BannedMules;
        
        public int BannedBots { get; set; }
        
        public int BannedMules { get; set; }
    }
}
