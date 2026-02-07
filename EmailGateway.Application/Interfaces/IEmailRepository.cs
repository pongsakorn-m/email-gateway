using EmailGateway.Domain.Entities;

namespace EmailGateway.Application.Interfaces;

public interface IEmailRepository
{
    Task<Email?> GetByIdempotencyKeyAsync(string idempotencyKey);
    Task<List<Email>> GetPendingAsync(int batchSize);
    Task AddAsync(Email email);
    Task UpdateAsync(Email email);
}