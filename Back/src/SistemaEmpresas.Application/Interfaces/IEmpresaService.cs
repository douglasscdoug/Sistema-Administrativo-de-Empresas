using SistemaEmpresas.Application.DTOs;

namespace SistemaEmpresas.Application.Interfaces;

public interface IEmpresaService
{
    Task<EmpresaResponseDto[]> GetAllAsync();
    Task<EmpresaResponseDto?> GetByIdAsync(Guid id);
    Task<EmpresaResponseDto> AddAsync(EmpresaRequestDto empresaRequestDto);
    Task<EmpresaResponseDto?> UpdateAsync(Guid id, EmpresaRequestDto empresaRequestDto);
    Task<bool> DeleteAsync(Guid id);
}
