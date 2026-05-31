using Submission.Domain.ValueObjects;

namespace Submission.Domain.Entities;

public partial class Author
{
    public static Author Create(string firstName, string lastName, string email, string? title, string affiliation)
    {
        return new Author
        {
            Email = EmailAddress.Create(email),
            FirstName = firstName,
            LastName = lastName,
            Title = title,
            Affiliation = affiliation
        };

        //TODO: Emit Create Author Event
    }
}
