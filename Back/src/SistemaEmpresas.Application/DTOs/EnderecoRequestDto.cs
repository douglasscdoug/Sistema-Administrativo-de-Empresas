namespace SistemaEmpresas.Application.DTOs;

public class EnderecoRequestDto
{
    public required string Logradouro { get; set; } = null!;
    public required string Numero { get; set; } = null!;
    public string? Complemento { get; set; }
    public required string Bairro { get; set; } = null!;
    public required string Cidade { get; set; } = null!;
    public required string Estado { get; set; } = null!;
    public required string Cep { get; set; } = null!;
}
