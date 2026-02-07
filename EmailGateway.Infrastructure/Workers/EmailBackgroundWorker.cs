using EmailGateway.Application.Interfaces;
using EmailGateway.Application.Models;
using EmailGateway.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EmailGateway.Infrastructure.Workers;

public class EmailBackgroundWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<EmailBackgroundWorker> _logger;

    private const int BatchSize = 10;
    private static readonly TimeSpan PollingInterval = TimeSpan.FromSeconds(5);

    public EmailBackgroundWorker(
        IServiceScopeFactory scopeFactory,
        ILogger<EmailBackgroundWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Email background worker started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();

                var emailRepository = scope.ServiceProvider
                    .GetRequiredService<IEmailRepository>();

                var emailSender = scope.ServiceProvider
                    .GetRequiredService<IEmailSender>();

                var deadLetterRepository = scope.ServiceProvider
                    .GetRequiredService<IDeadLetterRepository>();
                
                var emails = await emailRepository.GetPendingAsync(BatchSize);

                if (emails.Count == 0)
                {
                    await Task.Delay(PollingInterval, stoppingToken);
                    continue;
                }

                foreach (var email in emails)
                {
                    if (!email.CanRetry())
                    {
                        _logger.LogWarning(
                            "Email {EmailId} exceeded max retry count and will be skipped",
                            email.Id);
                        continue;
                    }

                    try
                    {
                        _logger.LogInformation(
                            "Sending email {EmailId} to {Recipient}",
                            email.Id,
                            email.To);

                        await emailSender.SendAsync(email, stoppingToken);

                        email.MarkSent();
                        await emailRepository.UpdateAsync(email);

                        _logger.LogInformation(
                            "Email {EmailId} sent successfully",
                            email.Id);
                    }
                    catch (Exception ex)
                    {
                        email.MarkFailed();
                        await emailRepository.UpdateAsync(email);

                        if (email.IsDead())
                        {
                            await deadLetterRepository.AddAsync(new DeadLetterEmail
                            {
                                EmailId = email.Id,
                                To = email.To,
                                Template = email.Template,
                                RetryCount = email.RetryCount,
                                FailureReason = ex.Message,
                                FailedAt = DateTime.UtcNow
                            });

                            _logger.LogError(ex,
                                "Email {EmailId} moved to dead-letter after {RetryCount} retries",
                                email.Id,
                                email.RetryCount);
                        }
                        else
                        {
                            _logger.LogWarning(ex,
                                "Failed to send email {EmailId}, retry {RetryCount}/{MaxRetry}",
                                email.Id,
                                email.RetryCount,
                                Email.MaxRetryCount);
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in EmailBackgroundWorker");
            }

            await Task.Delay(PollingInterval, stoppingToken);
        }

        _logger.LogInformation("Email background worker stopped");
    }
}