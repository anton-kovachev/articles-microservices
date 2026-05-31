namespace EmailService.Contract;

public interface IEmailService
{
    Task<bool> SendEmailAsync(EmailMessage emailMessage, CancellationToken cancellationToken);
}

