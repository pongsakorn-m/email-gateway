using EmailGateway.Application.Commands;
using EmailGateway.Application.Models;

namespace EmailGateway.Application.Interfaces;

public interface IEmailService
{
    Task<QueueEmailResult> QueueAsync(SendEmailCommand command);
}