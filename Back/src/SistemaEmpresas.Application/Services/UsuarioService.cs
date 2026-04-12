using AutoMapper;
using SistemaEmpresas.Application.DTOs;
using SistemaEmpresas.Application.Exceptions;
using SistemaEmpresas.Application.Interfaces;
using SistemaEmpresas.Application.Security;
using SistemaEmpresas.Domain.Entities;
using SistemaEmpresas.Infrastructure.Repositories;

namespace SistemaEmpresas.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly UsuarioRepository _usuarioRepository;
    private readonly IMapper _mapper;

    public UsuarioService(UsuarioRepository usuarioRepository, IMapper mapper)
    {
        _usuarioRepository = usuarioRepository;
        _mapper = mapper;
    }

    public async Task<UsuarioResponseDto[]> GetAllAsync()
    {
        var usuarios = await _usuarioRepository.GetAllAsync();
        return _mapper.Map<UsuarioResponseDto[]>(usuarios);
    }

    public async Task<UsuarioResponseDto?> GetByIdAsync(Guid id)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id);
        if (usuario == null) return null;

        return _mapper.Map<UsuarioResponseDto>(usuario);
    }

    public async Task<UsuarioResponseDto> AddAsync(UsuarioRequestDto dto)
    {
        var usuarioexistente = await _usuarioRepository.GetByEmailAsync(dto.Email);
        if (usuarioexistente != null)
            throw new BusinessException("Email", "Já existe um usuário com este e-mail.");

        var usuario = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            SenhaHash = PasswordHasher.Hash(dto.Senha),
            Ativo = true,
            DataCriacao = DateTime.Now
        };

        await _usuarioRepository.AddAsync(usuario);
        var sucess = await _usuarioRepository.SaveChangesAsync();

        if (!sucess)
            throw new BusinessException("Erro", "Ocorreu um erro ao salvar o usuário.");

        return _mapper.Map<UsuarioResponseDto>(usuario);
    }

    public async Task<UsuarioResponseDto?> UpdateAsync(Guid id, UsuarioRequestDto dto)
    {
        var usuario = await _usuarioRepository.GetByIdForUpdateAsync(id);
        if (usuario == null) return null;

        if(usuario.Email != dto.Email)
        {
            var usuarioexistente = await _usuarioRepository.GetByEmailAsync(dto.Email);
            if (usuarioexistente != null)
                throw new BusinessException("Email", "Ja existe um usuario com este e-mail.");
        }

        usuario.Nome = dto.Nome;
        usuario.Email = dto.Email;
        usuario.SenhaHash = PasswordHasher.Hash(dto.Senha);
        usuario.Ativo = true;

        var sucess = await _usuarioRepository.SaveChangesAsync();
        if (!sucess)
            throw new BusinessException("Erro", "Ocorreu um erro ao atualizar o usuário.");

        return _mapper.Map<UsuarioResponseDto>(usuario);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var usuario = await _usuarioRepository.GetByIdForUpdateAsync(id);
        if (usuario == null) return false;

        usuario.Ativo = false;

        return await _usuarioRepository.SaveChangesAsync();
    }
}
