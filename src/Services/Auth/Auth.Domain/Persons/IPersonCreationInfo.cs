using Articles.Abstractions.Enums;
using Auth.Domain.Users;

namespace Auth.Domain.Persons;

public interface IPersonCreationInfo
{
    string FirstName { get; }
    string LastName { get; }
    Gender Gender { get; }
    string? PictureUrl { get; }
    Honorific? Honorific { get; }
    string? Position { get; }
    string? CompanyName { get; }
    string? Affiliation { get; }
}
