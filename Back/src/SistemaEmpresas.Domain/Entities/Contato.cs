namespace SistemaEmpresas.Domain.Entities;

public class Contato
{
    public Guid Id { get; set; }
    public required string Nome { get; set; }
    public required string Email { get; set; }
    public required string Telefone { get; set; }
    public Guid EmpresaId { get; set; }
}
