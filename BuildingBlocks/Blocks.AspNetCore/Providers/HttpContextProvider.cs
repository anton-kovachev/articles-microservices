using Blocks.AspNetCore.Extensions;
using Blocks.Core.Extensions;
using Blocks.Core.Security;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Blocks.AspNetCore.Providers;

public class HttpContextProvider(IHttpContextAccessor _httpConttAccessor) : IClaimsProvider
{
    public string GetClaimValue(string claimName)
        => TryGetClaimValue(claimName) ?? throw new InvalidOperationException($"Missing claim: {claimName}.");

    public string? TryGetClaimValue(string claimName)
    => _httpConttAccessor.GetClaimValue(claimName);

    public string GetUserEmail()
        => GetClaimValue(ClaimTypes.Email);

    public int GetUserId()
        => TryGetUserId() ?? throw new UnauthorizedAccessException($"Missing claim: {ClaimTypes.NameIdentifier}");

    public int? TryGetUserId()
        => TryGetClaimValue("sub")?.ToInteger();

    public string GetUserName()
        => GetClaimValue(ClaimTypes.NameIdentifier);

    public string GetUserRole()
        => GetClaimValue(ClaimTypes.Role);
}
