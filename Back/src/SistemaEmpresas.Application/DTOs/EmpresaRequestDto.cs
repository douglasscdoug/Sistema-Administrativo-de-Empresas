namespace SistemaEmpresas.Application.DTOs;

public class EmpresaRequestDto
{
    public required string RazaoSocial { get; set; } = null!;
    public required string Cnpj { get; set; } = null!;
    public bool Ativo { get; set; }
    public EnderecoRequestDto Endereco { get; set; } = null!;
    public ContatoRequestDto Contato { get; set; } = null!;
}