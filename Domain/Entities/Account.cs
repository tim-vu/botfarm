using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json;
using FORFarm.Domain.Enums;
using FORFarm.Domain.ValueObjects;

namespace FORFarm.Domain.Entities
{
    public class Account
    {
        public int ID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public bool Mule { get; set; }

        public DateTime MemberExpirationDate { get; set; }

        public TimeSpan Runtime { get; set; }
        
        public bool Banned { get; set; }

        public ICollection<Skill> Skills { get; set; } = new HashSet<Skill>();

        public int? ProxyId { get; set; }
        public virtual Proxy Proxy { get; set; }
        
        public int GoldEarned { get; set; }
    }
}
