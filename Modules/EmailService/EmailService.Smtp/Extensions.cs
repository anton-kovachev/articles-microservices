using EmailService.Contract;
using MimeKit;

namespace EmailService.Smtp;

public static class Extensions
{
    public static MailboxAddress ToMailboxAddress(this EmailAddress emailAddress)
        => new MailboxAddress(emailAddress.Name, emailAddress.Address);

    public static MimeMessage ToMailKitMessage(this EmailMessage emailMessage)
    {
        var mimeMessage = new MimeMessage();
        mimeMessage.Subject = emailMessage.Subject;
        mimeMessage.From.Add(emailMessage.From.ToMailboxAddress());
        mimeMessage.To.AddRange(emailMessage.To.Select(t => t.ToMailboxAddress()).ToList());

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = emailMessage.Content.Value
        };

        mimeMessage.Body = bodyBuilder.ToMessageBody();
        return mimeMessage;
    }
}
