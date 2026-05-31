namespace EmailService.Contract;

public enum ContentType
{
    Text,
    Html
}

public record EmailContent(ContentType Type, string Value);
public record EmailAddress(string Name, string Address);
public record EmailMessage(string Subject, EmailContent Content, EmailAddress From, List<EmailAddress> To);
