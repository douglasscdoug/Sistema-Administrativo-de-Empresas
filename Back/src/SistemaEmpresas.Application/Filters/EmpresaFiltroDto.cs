using SistemaEmpresas.Application.Common.Utils;

namespace SistemaEmpresas.Application.Filters;

public class EmpresaFiltroDto : PagedRequest
{
    public string? RazaoSocial { get; set; }
    public string? Cnpj { get; set; }
    public bool? Ativo { get; set; }
}
