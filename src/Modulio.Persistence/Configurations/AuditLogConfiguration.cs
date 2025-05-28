using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modulio.Domain.Base;

namespace Modulio.Persistence.Configurations
{
    public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("AuditLogs");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Action)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.EntityType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.EntityId)
                .HasMaxLength(50);

            builder.Property(x => x.UserId)
                .HasMaxLength(50);

            builder.Property(x => x.UserName)
                .HasMaxLength(100);

            builder.Property(x => x.IpAddress)
                .HasMaxLength(45); // IPv6 max length

            builder.Property(x => x.Status)
                .IsRequired()
                .HasMaxLength(20);

            // Indexes for performance
            builder.HasIndex(x => x.EntityType);
            builder.HasIndex(x => x.EntityId);
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.Timestamp);
        }
    }
}