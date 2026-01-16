namespace SistemaEmpresas.Application.DTOs;

public class ContatoRequestDto
{
    public required string Nome { get; set; } = null!;
    public required string Email { get; set; } = null!;
    public required string Telefone { get; set; } = null!;
}
