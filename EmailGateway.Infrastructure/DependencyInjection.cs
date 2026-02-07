using EmailGateway.Application.Interfaces;
using EmailGateway.Infrastructure.EmailSender;
using EmailGateway.Infrastructure.Persistence;
using EmailGateway.Infrastructure.Repositories;
using EmailGateway.Infrastructure.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmailGateway.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<EmailDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("Default")));

        services.AddScoped<IEmailRepository, EmailRepository>();
        services.AddScoped<IDeadLetterRepository, DeadLetterRepository>();
        services.AddScoped<IEmailSender, SmtpEmailSender>();
        services.AddHostedService<EmailBackgroundWorker>();
        
        return services;
    }
}