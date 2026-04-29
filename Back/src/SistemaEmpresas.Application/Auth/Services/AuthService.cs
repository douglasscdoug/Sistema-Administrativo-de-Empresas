using Microsoft.Extensions.Logging;
using SistemaEmpresas.Application.Auth.Interfaces;
using SistemaEmpresas.Application.DTOs.Auth;
using SistemaEmpresas.Application.Exceptions;
using SistemaEmpresas.Domain.Entities;
using SistemaEmpresas.Infrastructure.Repositories;

namespace SistemaEmpresas.Application.Auth.Services;

public class AuthService : IAuthService
{
    private readonly UsuarioRepository _usuarioRepository;
    private readonly ITokenService _tokenService;
    private readonly RefreshTokenRepository _refreshTokenRepository;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        UsuarioRepository usuarioRepository,
        ITokenService tokenService,
        RefreshTokenRepository refreshTokenRepository,
        ILogger<AuthService> logger
    )
    {
        _usuarioRepository = usuarioRepository;
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
        _logger = logger;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        _logger.LogInformation("Iniciando processo autenticação.");

        var usuario = await _usuarioRepository.GetByEmailAsync(request.Email);

        if (usuario == null || !usuario.Ativo)
        {
            _logger.LogWarning("Tentativa de login com email inexistente ou inativo.");
            throw new UnauthorizedException("Não existe usuário com este e-mail.");
        }

        var senhaValida = BCrypt.Net.BCrypt.Verify(request.Senha, usuario.SenhaHash);

        if (!senhaValida)
        {
            _logger.LogWarning("Tentativa de login com senha incorreta.");
            throw new UnauthorizedException("Senha incorreta.");
        }

        var token = _tokenService.GerarToken(usuario);
        var refreshToken = _tokenService.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            UsuarioId = usuario.Id,
            ExpirationDate = DateTime.UtcNow.AddDays(7)
        };

        await _refreshTokenRepository.AddAsync(refreshTokenEntity);
        await _refreshTokenRepository.SaveChangesAsync();

        _logger.LogInformation("Login bem-sucedido.");

        return new LoginResponseDto
        {
            Token = token,
            RefreshToken = refreshToken
        };
    }

    public async Task<LoginResponseDto> RefreshTokenAsync(string refreshToken)
    {
        _logger.LogInformation("Iniciando processo de refresh token.");
        var token = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

        if (token == null)
        {
            _logger.LogWarning("Tentativa de refresh token com token inexistente.");
            throw new UnauthorizedException("Refresh token inválido.");
        }
        if (token.Revoked)
        {
            _logger.LogWarning("Tentativa de refresh token com token revogado.");
            throw new UnauthorizedException("Refresh token revogado.");
        }
        if (token.ExpirationDate < DateTime.UtcNow)
        {
            _logger.LogWarning("Tentativa de refresh token com token expirado.");
            throw new UnauthorizedException("Refresh token expirado.");
        }

        token.Revoked = true;

        var novoAccessToken = _tokenService.GerarToken(token.Usuario);
        var novoRefreshToken = _tokenService.GenerateRefreshToken();

        await _refreshTokenRepository.AddAsync(new RefreshToken
        {
            Token = novoRefreshToken,
            UsuarioId = token.UsuarioId,
            ExpirationDate = DateTime.UtcNow.AddDays(7)
        });

        await _refreshTokenRepository.SaveChangesAsync();

        _logger.LogInformation(
            "Refresh token realizado com sucesso para usuário {UsuarioId}",
            token.UsuarioId
        );

        return new LoginResponseDto
        {
            Token = novoAccessToken,
            RefreshToken = novoRefreshToken
        };
    }

    public async Task LogoutAsync(string refreshToken)
    {
        var token = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

        if (token == null)
        {
            _logger.LogWarning("Tentativa de logout com refresh token inexistente.");
            return;
        }

        token.Revoked = true;

        await _refreshTokenRepository.SaveChangesAsync();

        _logger.LogInformation(
            "Logout realizado com sucesso para usuário {UsuarioId}",
            token.UsuarioId
        );
    }
}