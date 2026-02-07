using EmailGateway.Application.Interfaces;
using EmailGateway.Domain.Entities;
using EmailGateway.Domain.Enums;
using EmailGateway.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EmailGateway.Infrastructure.Repositories;

public class EmailRepository : IEmailRepository
{
    private readonly EmailDbContext _context;

    public EmailRepository(EmailDbContext context)
    {
        _context = context;
    }

    public async Task<Email?> GetByIdempotencyKeyAsync(string idempotencyKey)
    {
        return await _context.Emails
            .FirstOrDefaultAsync(e => e.IdempotencyKey == idempotencyKey);
    }
    
    public async Task<List<Email>> GetPendingAsync(int batchSize)
    {
        return await _context.Emails
            .Where(e =>
                (e.Status == EmailStatus.Queued ||
                 e.Status == EmailStatus.Failed) &&
                e.RetryCount < Email.MaxRetryCount)
            .OrderBy(e => e.Id)
            .Take(batchSize)
            .ToListAsync();
    }

    public async Task AddAsync(Email email)
    {
        _context.Emails.Add(email);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(Email email)
    {
        _context.Emails.Update(email);
        await _context.SaveChangesAsync();
    }
}