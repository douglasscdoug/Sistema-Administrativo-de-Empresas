namespace SistemaEmpresas.Domain.Entities;

public class Empresa
{
    public Guid Id { get; set; }
    public required string RazaoSocial { get; set; }    
    public required string Cnpj { get; set; }
    public bool Status { get; set; }
    public required Endereco Endereco { get; set; }
    public required Contato Contato { get; set; }
}
