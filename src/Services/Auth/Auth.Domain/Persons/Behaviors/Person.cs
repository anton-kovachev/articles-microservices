using Auth.Domain.Persons.ValueObjects;
using Auth.Domain.Users;
using Blocks.Core.Extensions;

namespace Auth.Domain.Persons;

public partial class Person
{
    public static Person Create(IUserCreationInfo userInfo)
    {
        if (userInfo.UserRoles.IsNullOrEmpty())
            throw new Exception();

        var person= new Person
        {
            FirstName = userInfo.FirstName,
            LastName = userInfo.LastName,
            Email = userInfo.Email,
            Gender = userInfo.Gender,
            PictureUrl = userInfo.PictureUrl,
            Honorific = HonorificTitle.FromEnum(userInfo.Honorific),
            ProfessionalProfile = ProfessionalProfile.Create(userInfo.Position, userInfo.CompanyName, userInfo.Affiliation),
        };

        //TODO: Add User Created domain event
        return person;
    }

    public void AssignUser(User user)
    {
        this.UserId = user.Id;
        this.Email = user.NormalizedEmail!;
    }
}
