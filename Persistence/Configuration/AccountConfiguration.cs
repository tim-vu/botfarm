using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using FORFarm.Domain.Entities;
using FORFarm.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(a => a.ID);

            builder.Property(a => a.Username).IsRequired();
            builder.Property(a => a.Password).IsRequired();

            builder.OwnsMany(a => a.Skills, s =>
            {
                s.WithOwner().HasForeignKey("OwnerId");
                s.HasKey("Type", "OwnerId");
                s.Property(i => i.Level);
            });
        }
    }
}
