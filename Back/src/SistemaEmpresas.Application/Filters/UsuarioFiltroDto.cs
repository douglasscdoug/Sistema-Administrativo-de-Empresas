using SistemaEmpresas.Application.Common.Utils;

namespace SistemaEmpresas.Application.Filters;

public class UsuarioFiltroDto : PagedRequest
{
    public string? Nome { get; set; }
    public string? Email { get; set; }
    public bool? Ativo { get; set; }
}
