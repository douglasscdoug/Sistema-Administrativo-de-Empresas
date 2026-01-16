namespace SistemaEmpresas.Application.DTOs;

public class EmpresaResponseDto
{
    public required Guid Id { get; set; }
    public required string RazaoSocial { get; set; } = null!;
    public required string Cnpj { get; set; } = null!;
    public bool Ativo { get; set; }
    public required EnderecoResponseDto Endereco { get; set; } = null!;
    public required ContatoResponseDto Contato { get; set; } = null!;
}
