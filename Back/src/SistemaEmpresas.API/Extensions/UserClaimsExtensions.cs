using System.Security.Claims;

namespace SistemaEmpresas.API.Extensions;

public static class UserClaimsExtensions
{
    public static string? GetUserId(this ClaimsPrincipal user) =>
        user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    public static string? GetUserRole(this ClaimsPrincipal user) =>
        user.FindFirst(ClaimTypes.Role)?.Value;
}
