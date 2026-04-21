using SistemaEmpresas.Domain.Enums;

namespace SistemaEmpresas.Domain.Entities;

public class Usuario
{
    public Guid Id { get; set; }
    public required string Nome { get; set; }
    public required string Email { get; set; }
    public required string SenhaHash { get; set; }
    public UsuarioRole Role { get; set; }
    // TODO: Futuro uso para permissões granulares (claims-based authorization)
    public List<string> Claims { get; set; } = new List<String>();
    public bool Ativo { get; set; } = true;
    public DateTime DataCriacao { get; set; }
}