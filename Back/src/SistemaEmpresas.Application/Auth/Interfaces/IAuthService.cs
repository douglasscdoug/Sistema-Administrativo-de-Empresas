using SistemaEmpresas.Application.DTOs.Auth;

namespace SistemaEmpresas.Application.Auth.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
}
