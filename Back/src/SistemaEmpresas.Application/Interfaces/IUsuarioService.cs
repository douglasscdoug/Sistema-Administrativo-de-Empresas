using SistemaEmpresas.Application.Common.Utils;
using SistemaEmpresas.Application.DTOs;
using SistemaEmpresas.Application.Filters;

namespace SistemaEmpresas.Application.Interfaces;

public interface IUsuarioService
{
    Task<PagedResult<UsuarioResponseDto>> Filtrar(UsuarioFiltroDto filtro);
    Task<UsuarioResponseDto?> GetByIdAsync(Guid id);
    Task<UsuarioResponseDto> AddAsync(UsuarioRequestDto dto);
    Task<UsuarioResponseDto?> UpdateAsync(Guid id, UsuarioRequestDto dto);
    Task<bool> DeleteAsync(Guid id);
}
