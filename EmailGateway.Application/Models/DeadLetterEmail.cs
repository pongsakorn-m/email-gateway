namespace EmailGateway.Application.Models;

public class DeadLetterEmail
{
    public Guid EmailId { get; init; }
    public string To { get; init; } = null!;
    public string Template { get; init; } = null!;
    public int RetryCount { get; init; }
    public string FailureReason { get; init; } = null!;
    public DateTime FailedAt { get; init; }
}