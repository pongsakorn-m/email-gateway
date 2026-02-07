using EmailGateway.Application.Interfaces;
using EmailGateway.Application.Models;
using EmailGateway.Domain.Entities;
using EmailGateway.Infrastructure.Persistence;

namespace EmailGateway.Infrastructure.Repositories;

public class DeadLetterRepository : IDeadLetterRepository
{
    private readonly EmailDbContext _context;

    public DeadLetterRepository(EmailDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(DeadLetterEmail deadLetter)
    {
        var entity = new DeadLetterEmailEntity
        {
            Id = Guid.NewGuid(),
            EmailId = deadLetter.EmailId,
            To = deadLetter.To,
            Template = deadLetter.Template,
            RetryCount = deadLetter.RetryCount,
            FailureReason = deadLetter.FailureReason,
            FailedAt = deadLetter.FailedAt
        };

        _context.DeadLetterEmails.Add(entity);
        await _context.SaveChangesAsync();
    }
}