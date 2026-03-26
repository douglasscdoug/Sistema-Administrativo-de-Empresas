using SistemaEmpresas.Application.DTOs;

namespace SistemaEmpresas.Application.Interfaces;

public interface IUsuarioService
{
    Task<UsuarioResponseDto[]> GetAllAsync();
    Task<UsuarioResponseDto?> GetByIdAsync(Guid id);
    Task<UsuarioResponseDto> AddAsync(UsuarioRequestDto dto);
}
