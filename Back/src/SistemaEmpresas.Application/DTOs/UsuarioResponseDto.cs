using System;

namespace SistemaEmpresas.Application.DTOs;

public class UsuarioResponseDto
{
    public Guid Id { get; set; }
    public required string Nome { get; set; }
    public required string Email { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
}
