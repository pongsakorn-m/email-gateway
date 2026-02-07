using EmailGateway.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmailGateway.Infrastructure.Persistence.Configurations;

public class EmailEntityConfiguration : IEntityTypeConfiguration<Email>
{
    public void Configure(EntityTypeBuilder<Email> builder)
    {
        builder.ToTable("emails");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.To)
            .IsRequired()
            .HasMaxLength(320);

        builder.Property(e => e.Template)
            .IsRequired();

        builder.Property(e => e.Status)
            .IsRequired();

        builder.Property(e => e.RetryCount)
            .IsRequired();

        builder.Property(e => e.IdempotencyKey)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(e => e.IdempotencyKey)
            .IsUnique();
    }
}