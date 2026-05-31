
using Articles.Abstractions.Enums;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Auth.Domain.Roles;

public partial class Role : IdentityRole<int>, IEntity<int>
{
    public required UserRoleType Type { get; set; }
    public required string Description { get; set; }
}
