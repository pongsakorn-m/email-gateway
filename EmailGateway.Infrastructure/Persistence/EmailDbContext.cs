using EmailGateway.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmailGateway.Infrastructure.Persistence;

public class EmailDbContext : DbContext
{
    public DbSet<Email> Emails => Set<Email>();
    public DbSet<DeadLetterEmailEntity> DeadLetterEmails => Set<DeadLetterEmailEntity>();


    public EmailDbContext(DbContextOptions<EmailDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(EmailDbContext).Assembly);
    }
}