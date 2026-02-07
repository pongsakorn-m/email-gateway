using EmailGateway.Application.Commands;
using EmailGateway.Application.Interfaces;
using EmailGateway.Application.Models;
using EmailGateway.Domain.Entities;

namespace EmailGateway.Application.Services;

public class EmailService : IEmailService
{
    private readonly IEmailRepository _repository;

    public EmailService(IEmailRepository repository)
    {
        _repository = repository;
    }

    public async Task<QueueEmailResult> QueueAsync(SendEmailCommand command)
    {
        var existing =
            await _repository.GetByIdempotencyKeyAsync(command.IdempotencyKey);

        if (existing != null)
        {
            return new QueueEmailResult(existing.Id);
        }

        var email = new Email(
            command.To,
            command.Template,
            command.IdempotencyKey);

        await _repository.AddAsync(email);

        return new QueueEmailResult(email.Id);
    }
}