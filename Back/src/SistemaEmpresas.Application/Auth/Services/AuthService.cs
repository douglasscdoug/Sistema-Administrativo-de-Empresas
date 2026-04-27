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

    public AuthService(
        UsuarioRepository usuarioRepository,
        ITokenService tokenService,
        RefreshTokenRepository refreshTokenRepository
    )
    {
        _usuarioRepository = usuarioRepository;
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        var usuario = await _usuarioRepository.GetByEmailAsync(request.Email);

        if (usuario == null || !usuario.Ativo)
            throw new UnauthorizedException("Não existe usuário com este e-mail.");

        var senhaValida = BCrypt.Net.BCrypt.Verify(request.Senha, usuario.SenhaHash);

        if (!senhaValida)
            throw new UnauthorizedException("Senha incorreta.");

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

        return new LoginResponseDto
        {
            Token = token,
            RefreshToken = refreshToken
        };
    }

    public async Task<LoginResponseDto> RefreshTokenAsync(string refreshToken)
    {
        var token = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

        if (token == null) throw new UnauthorizedException("Refresh token inválido.");
        if (token.Revoked) throw new UnauthorizedException("Refresh token revogado.");
        if (token.ExpirationDate < DateTime.UtcNow) throw new UnauthorizedException("Refresh token expirado.");

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

        return new LoginResponseDto
        {
            Token = novoAccessToken,
            RefreshToken = novoRefreshToken
        };
    }

    public async Task LogoutAsync(string refreshToken)
    {
        var token = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

        if (token != null)
        {
            token.Revoked = true;
            await _refreshTokenRepository.SaveChangesAsync();
        }
    }
}