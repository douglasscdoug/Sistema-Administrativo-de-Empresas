using SistemaEmpresas.Application.Exceptions;
using SistemaEmpresas.Application.Security;
using SistemaEmpresas.Domain.Entities;
using SistemaEmpresas.Infrastructure.Repositories;

namespace SistemaEmpresas.Application.Services;

public class AuthService
{
    private readonly UsuarioRepository _usuarioReposritory;

    public AuthService(UsuarioRepository usuarioRepository)
    {
        _usuarioReposritory = usuarioRepository;
    }

    public async Task<Usuario> ValidarUsuarioAsync(string email, string senha)
    {
        var usuario = await _usuarioReposritory.GetByEmailAsync(email);

        if (usuario == null)
            throw new BusinessException("Usuário ou senha inválidos");

        if (!PasswordHasher.Verify(senha, usuario.SenhaHash))
            throw new BusinessException("Usuário ou senha inválidos");

        return usuario;
    }
}
