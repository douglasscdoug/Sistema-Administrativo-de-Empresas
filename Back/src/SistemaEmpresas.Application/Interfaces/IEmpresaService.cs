using SistemaEmpresas.Application.Common.Utils;
using SistemaEmpresas.Application.DTOs;
using SistemaEmpresas.Application.Filters;

namespace SistemaEmpresas.Application.Interfaces;

public interface IEmpresaService
{
    Task<PagedResult<EmpresaResponseDto>> Filtrar(EmpresaFiltroDto filtro);
    Task<EmpresaResponseDto?> GetByIdAsync(Guid id);
    Task<EmpresaResponseDto> AddAsync(EmpresaRequestDto dto);
    Task<EmpresaResponseDto?> UpdateAsync(Guid id, EmpresaRequestDto dto);
    Task<bool> DeleteAsync(Guid id);
}
