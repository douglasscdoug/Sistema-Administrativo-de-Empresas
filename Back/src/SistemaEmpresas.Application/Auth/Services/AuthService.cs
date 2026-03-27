using SistemaEmpresas.Application.Auth.Interfaces;
using SistemaEmpresas.Application.DTOs.Auth;
using SistemaEmpresas.Application.Exceptions;
using SistemaEmpresas.Infrastructure.Repositories;

namespace SistemaEmpresas.Application.Auth.Services;

public class AuthService : IAuthService
{
    private readonly UsuarioRepository _usuarioRepository;
    private readonly ITokenService _tokenService;

    public AuthService(UsuarioRepository usuarioRepository, ITokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
        }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        var usuario = await _usuarioRepository.GetByEmailAsync(request.Email);

        if (usuario == null || !usuario.Ativo)
            throw new BusinessException("Usuário ou senha inválidos.");

        var senhaValida = BCrypt.Net.BCrypt.Verify(request.Senha, usuario.SenhaHash);

        if (!senhaValida)
            throw new BusinessException("Usuário ou senha inválidos.");

        var token = _tokenService.GerarToken(usuario);

        return new LoginResponseDto
        {
            Token = token
        };
    }
}
