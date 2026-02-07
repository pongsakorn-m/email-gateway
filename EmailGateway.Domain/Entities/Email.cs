using EmailGateway.Domain.Enums;

namespace EmailGateway.Domain.Entities;

public class Email
{
    public Guid Id { get; private set; }
    public string To { get; private set; }
    public string Template { get; private set; }
    public EmailStatus Status { get; private set; }
    public int RetryCount { get; private set; }
    public string IdempotencyKey { get; private set; }
    
    private Email() { } // For ORM later, still clean

    public Email(string to, string template, string idempotencyKey)
    {
        Id = Guid.NewGuid();
        To = to;
        Template = template;
        IdempotencyKey = idempotencyKey;
        Status = EmailStatus.Queued;
        RetryCount = 0;
    }

    public void MarkSent()
    {
        Status = EmailStatus.Sent;
    }

    public void MarkFailed()
    {
        RetryCount++;
        Status = EmailStatus.Failed;
    }
    
    public const int MaxRetryCount = 3;

    public bool CanRetry()
    {
        return RetryCount < MaxRetryCount;
    }
    
    public bool IsDead()
    {
        return RetryCount >= MaxRetryCount && Status == EmailStatus.Failed;
    }
}