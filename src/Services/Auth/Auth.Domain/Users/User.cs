
using Microsoft.AspNetCore.Identity;
using Domain.Entities;
using Auth.Domain.Persons;

namespace Auth.Domain.Users;

public partial class User : IdentityUser<int>, IEntity<int>
{
    public DateTime RegistrationDate { get; init; } = DateTime.UtcNow;
    public DateTime? LastLogin { get; set; }
    public int PersonId { get; set; }
    public Person Person { get; set; } = null!;
    private List<UserRole> _userRoles = new();
    public virtual IReadOnlyList<UserRole> UserRoles => _userRoles.AsReadOnly();
    public List<RefreshToken> _refreshTokens = new();
    public virtual IReadOnlyList<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();
}
