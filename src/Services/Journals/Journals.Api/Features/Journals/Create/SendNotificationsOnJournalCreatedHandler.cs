using EmailService.Contract;
using FastEndpoints;
using Microsoft.Extensions.Options;
using Journals.Domain.Journal.Events;
using ContentType = EmailService.Contract.ContentType;

namespace Journals.Api.Features.Journals.Create
{
    public class SendNotificationsOnJournalCreatedHandler
        (IEmailService emailService, IHttpContextAccessor httpContextAccessor, IOptions<EmailOptions> emailOptions)
            : IEventHandler<JournalCreated>
    {
        public async Task HandleAsync(JournalCreated eventModel, CancellationToken cancellationToken)
    {
        var emailMessage = BuildEmailMessage(eventModel.Journal.ChiefEditorId,  emailOptions.Value.EmailFromAddress);
        await emailService.SendEmailAsync(emailMessage, cancellationToken);
    }

    public EmailMessage BuildEmailMessage(int chiefEditorId, string fromEmailAddress)

    {
        const string ConfirmationEmail =
            "Dear {0}, an account has beeen created for you. <br/> Please set your password using the following URL: <br/>{1}";

        return new EmailMessage(
            "Your Account Has Been Created - Set Your Password",
            new EmailContent(ContentType.Html, string.Format(ConfirmationEmail, string.Empty)),
            From: new EmailAddress("Articles", fromEmailAddress),
            To: new List<EmailAddress> { new EmailAddress(string.Empty, string.Empty) });
    }
}
}
