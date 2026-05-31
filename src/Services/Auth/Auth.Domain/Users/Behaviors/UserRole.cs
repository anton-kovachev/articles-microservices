using Mapster;
using Microsoft.AspNetCore.Identity;

namespace Auth.Domain.Users;

public partial class UserRole : IdentityUserRole<int>
{
    public static UserRole Create(IUserRole userRoleInfo)
    {
        var now = DateTime.UtcNow;

        if (userRoleInfo.StartDate.HasValue && userRoleInfo.StartDate.Value < now)
            throw new ArgumentException("Start date must be today or in the future", nameof(userRoleInfo.StartDate));

        if (userRoleInfo.ExpiringDate.HasValue && userRoleInfo.StartDate.HasValue && userRoleInfo.ExpiringDate.Value < userRoleInfo.StartDate.Value)
            throw new ArgumentException("Expiring date must be after the start date", nameof(userRoleInfo.ExpiringDate));

        var userRole = userRoleInfo.Adapt<UserRole>();
        return userRole;
    }
}
