namespace EmailGateway.Api.Contracts;

public record SendEmailRequest(
    string To,
    string Template
);