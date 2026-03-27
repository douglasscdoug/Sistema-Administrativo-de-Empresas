using SistemaEmpresas.Domain.Entities;

namespace SistemaEmpresas.Application.Auth.Interfaces;

public interface ITokenService
{
    string GerarToken(Usuario usuario);
}
