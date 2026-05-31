using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Blocks.Core;
using EmailService.Contract;

namespace EmailService.Smtp;

public static class MailServiceRegistration
{
    public static IServiceCollection AddSmtpMailService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAndValiadateOptions<EmailOptions>(configuration);
        services.AddSingleton<IEmailService, SmtpEmailService>();

        return services;
    }
}
