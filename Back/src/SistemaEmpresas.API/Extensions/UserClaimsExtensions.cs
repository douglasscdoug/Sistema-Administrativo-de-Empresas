using System.Security.Claims;
using SistemaEmpresas.Application.Exceptions;

namespace SistemaEmpresas.API.Extensions;

public static class UserClaimsExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var claim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(claim))
            throw new UnauthorizedException("Usuário não autenticado.");

        if (!Guid.TryParse(claim, out var userId))
            throw new UnauthorizedException("Id do usuário inválido no token.");

        return userId;
    }

    public static string? GetUserRole(this ClaimsPrincipal user) =>
        user.FindFirst(ClaimTypes.Role)?.Value;
}
