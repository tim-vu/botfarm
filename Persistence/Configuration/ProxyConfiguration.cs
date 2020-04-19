using FORFarm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class ProxyConfiguration : IEntityTypeConfiguration<Proxy>
    {
        public void Configure(EntityTypeBuilder<Proxy> builder)
        {
            builder.HasKey(p => p.ID);
            builder.Property(p => p.Ip).IsRequired();
            builder.Property(p => p.Port).IsRequired();
            builder.Property(p => p.Username).HasMaxLength(20);
            builder.Property(p => p.Password).HasMaxLength(20);
        }
    }
}