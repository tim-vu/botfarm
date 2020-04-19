using FORFarm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration
{
    public class MuleRequestConfiguration : IEntityTypeConfiguration<MuleRequest>
    {
        public void Configure(EntityTypeBuilder<MuleRequest> builder)
        {
            builder.HasKey(r => new {r.BotId, r.MuleId});
        }
    }
}