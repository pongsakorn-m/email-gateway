using EmailGateway.Api.Contracts;
using EmailGateway.Application.Commands;
using EmailGateway.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmailGateway.Api.Controllers;

[ApiController]
[Route("api/emails")]
public class EmailsController : ControllerBase
{
    private readonly IEmailService _emailService;

    public EmailsController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost]
    public async Task<IActionResult> SendEmail(
        [FromHeader(Name = "Idempotency-Key")] string idempotencyKey,
        [FromBody] SendEmailRequest request)
    {
        if (string.IsNullOrWhiteSpace(idempotencyKey))
        {
            return BadRequest("Idempotency-Key header is required.");
        }

        var result = await _emailService.QueueAsync(
            new SendEmailCommand(
                request.To,
                request.Template,
                idempotencyKey));

        return Accepted(new SendEmailResponse(
            result.EmailId,
            "Queued"));
    }
}