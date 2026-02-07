namespace EmailGateway.Domain.Entities;

public class DeadLetterEmailEntity
{
    public Guid Id { get; set; }
    public Guid EmailId { get; set; }
    public string To { get; set; } = null!;
    public string Template { get; set; } = null!;
    public int RetryCount { get; set; }
    public string FailureReason { get; set; } = null!;
    public DateTime FailedAt { get; set; }
}