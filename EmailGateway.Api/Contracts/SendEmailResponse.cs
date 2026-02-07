namespace EmailGateway.Api.Contracts;

public record SendEmailResponse(
    Guid EmailId,
    string Status    
);