using Microsoft.Extensions.Options;
using EmailService.Contract;
using MailKit.Net.Smtp;

namespace EmailService.Smtp;

public class SmtpEmailService : IEmailService
{
    private readonly EmailOptions _emailOptions;

    public SmtpEmailService(IOptions<EmailOptions> emailOptions)
    {
        _emailOptions = emailOptions.Value;
    }
        
    public async Task<bool> SendEmailAsync(EmailMessage emailMessage, CancellationToken cancellationToken)
    {
        var mailKitMessage = emailMessage.ToMailKitMessage();
        using var smtpClient = new SmtpClient();

        try
        {
            await smtpClient.ConnectAsync(_emailOptions.Smtp.Host, _emailOptions.Smtp.Port, _emailOptions.Smtp.UseSSL, cancellationToken);
            await smtpClient.AuthenticateAsync(_emailOptions.Smtp.Username, _emailOptions.Smtp.Password, cancellationToken);
            await smtpClient.SendAsync(mailKitMessage, cancellationToken);
        }
        catch (Exception ex)
        {
            //TODO: Log and rethrow
            return false;
        }

        return true;
    }
}
