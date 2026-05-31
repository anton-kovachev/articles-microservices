using Auth.Domain.Users.Events;
using Blocks.AspNetCore.Extensions;
using EmailService.Contract;
using Flurl;
using Microsoft.Extensions.Options;

namespace Auth.API.Features.Users.CreateUser;

public class SendConfirmationEmailOnUserCreatedHandler
    (IEmailService emailService, IHttpContextAccessor httpContextAccessor, IOptions<EmailOptions> emailOptions)
        : IEventHandler<UserCreated>
{
    public async Task HandleAsync(UserCreated eventModel, CancellationToken cancellationToken)
    {
        var url = httpContextAccessor?.HttpContext?.Request.BaseUrl()
            .AppendPathSegment("password").SetQueryParams(new { token = eventModel.ResetPasswordToken });
        var emailMessage = BuildEmailMessage(eventModel.User, url, emailOptions.Value.EmailFromAddress);
        await emailService.SendEmailAsync(emailMessage, cancellationToken);
    }

    public EmailMessage BuildEmailMessage(User user, string resetLink, string fromEmailAddress)
    {
        const string ConfirmationEmail =
            "Dear {0}, an account has beeen created for you. <br/> Please set your password using the following URL: <br/>{1}";

        return new EmailMessage(
            "Your Account Has Been Created - Set Your Password",
            new EmailContent(ContentType.Html, string.Format(ConfirmationEmail, user.Person.FullName, resetLink)),
            From: new EmailAddress("Articles", fromEmailAddress),
            To: new List<EmailAddress> { new EmailAddress(user.Person.FullName, user.Email!) });
    }
}
