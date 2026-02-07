using EmailGateway.Domain.Entities;

namespace EmailGateway.Application.Interfaces;

public interface IEmailSender
{
    Task SendAsync(Email email, CancellationToken ct);
}