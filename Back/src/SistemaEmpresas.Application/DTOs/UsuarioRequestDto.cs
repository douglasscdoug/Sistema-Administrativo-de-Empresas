using System;

namespace SistemaEmpresas.Application.DTOs;

public class UsuarioRequestDto
{
    public required string Nome { get; set; }
    public required string Email { get; set; }
    public required string Senha { get; set; }
}
