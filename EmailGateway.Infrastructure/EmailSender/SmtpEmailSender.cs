using System.Net.Mail;
using EmailGateway.Application.Interfaces;
using EmailGateway.Domain.Entities;

namespace EmailGateway.Infrastructure.EmailSender;

public class SmtpEmailSender : IEmailSender
{
    public async Task SendAsync(Email email, CancellationToken ct)
    {
        using var client = new SmtpClient("mailhog", 1025)
        {
            DeliveryMethod = SmtpDeliveryMethod.Network
        };

        var message = new MailMessage(
            "noreply@email-gateway.local",
            email.To,
            "Email Gateway",
            email.Template);

        await client.SendMailAsync(message, ct);
    }
}