using Articles.Abstractions.Enums;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Auth.Domain.Roles;

public partial class Role
{ 
    public static Role Create(UserRoleType userRoleType, DateTime? startDate, DateTime? expiringDate)
    {
        return new Role
        {
            Name = userRoleType.ToString(),
            Type = userRoleType,
            Description = $"Role for {userRoleType} users",
        };
    }
}

