namespace EmailGateway.Application.Commands;

public record SendEmailCommand(
    string To,
    string Template,
    string IdempotencyKey
);