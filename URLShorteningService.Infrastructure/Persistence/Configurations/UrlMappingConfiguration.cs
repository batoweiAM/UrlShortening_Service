using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URLShorteningService.Infrastructure.Persistence.Configurations
{
    public class UrlMappingConfiguration : IEntityTypeConfiguration<UrlMapping>
    {
        public void Configure(EntityTypeBuilder<UrlMapping> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.ShortCode)
                .HasMaxLength(8)
                .IsRequired();

            builder.Property(e => e.LongUrl)
                .HasMaxLength(2048)
                .IsRequired();

            builder.HasIndex(e => e.ShortCode)
                .IsUnique();

            builder.HasOne(e => e.Stats)
                .WithOne(e => e.UrlMapping)
                .HasForeignKey<UrlStats>(e => e.UrlMappingId);
        }
    }
}
