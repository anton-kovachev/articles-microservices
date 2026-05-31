using Articles.Abstractions.Enums;
using Auth.Domain.Persons;

namespace Auth.Domain.Users;

public interface IUserCreationInfo : IPersonCreationInfo
{
    string Email { get; }
    string? PhoneNumber { get; }
    IReadOnlyList<IUserRole> UserRoles { get; }
}

public interface IUserRole
{
    DateTime? ExpiringDate { get; }
    DateTime? StartDate { get; }
    UserRoleType Type { get; }
}
