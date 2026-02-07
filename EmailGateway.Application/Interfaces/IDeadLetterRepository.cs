using EmailGateway.Application.Models;

namespace EmailGateway.Application.Interfaces;

public interface IDeadLetterRepository
{
    Task AddAsync(DeadLetterEmail deadLetter);
}