using EmailGateway.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmailGateway.Infrastructure.Persistence.Configurations;

public class DeadLetterEmailConfiguration
    : IEntityTypeConfiguration<DeadLetterEmailEntity>
{
    public void Configure(EntityTypeBuilder<DeadLetterEmailEntity> builder)
    {
        builder.ToTable("dead_letter_emails");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.To).IsRequired();
        builder.Property(x => x.Template).IsRequired();
        builder.Property(x => x.FailureReason).IsRequired();
        builder.Property(x => x.FailedAt).IsRequired();
    }
}