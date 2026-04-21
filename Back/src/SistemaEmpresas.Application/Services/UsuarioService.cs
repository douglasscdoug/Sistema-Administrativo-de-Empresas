using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SistemaEmpresas.Application.Common.Utils;
using SistemaEmpresas.Application.DTOs;
using SistemaEmpresas.Application.Exceptions;
using SistemaEmpresas.Application.Filters;
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

    public async Task<PagedResult<UsuarioResponseDto>> Filtrar(UsuarioFiltroDto filtro)
    {
        var query = _usuarioRepository.Query();

        if (!string.IsNullOrWhiteSpace(filtro.Search))
        {
            query = query.Where(u =>
                u.Nome.Contains(filtro.Search) ||
                u.Email.Contains(filtro.Search));
        }

        if (!string.IsNullOrWhiteSpace(filtro.Nome))
        {
            var termo = filtro.Nome.Trim().ToLower();

            query = query.Where(u => u.Nome.ToLower().Contains(termo));
        }

        if (!string.IsNullOrWhiteSpace(filtro.Email))
        {
            var termo = filtro.Email.Trim().ToLower();

            query = query.Where(u => u.Email.ToLower().Contains(termo));
        }

        if (filtro.Ativo.HasValue)
            query = query.Where(u => u.Ativo == filtro.Ativo.Value);

        var total = await query.CountAsync();

        query = ApplyOrdering(query, filtro);

        var data = await query
            .Skip((filtro.Page - 1) * filtro.PageSize)
            .Take(filtro.PageSize)
            .ProjectTo<UsuarioResponseDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return new PagedResult<UsuarioResponseDto>
        {
            Data = data,
            Total = total,
            Page = filtro.Page,
            PageSize = filtro.PageSize
        };
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
            DataCriacao = DateTime.Now,
            Role = dto.Role
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
        usuario.Role = dto.Role;

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

    private IQueryable<Usuario> ApplyOrdering(IQueryable<Usuario> query, PagedRequest filtro)
    {
        if (string.IsNullOrWhiteSpace(filtro.OrderBy))
            return query.OrderBy(e => e.Id);

        return filtro.OrderBy.ToLower() switch
        {
            "nome" => filtro.Desc
                ? query.OrderByDescending(u => u.Nome)
                : query.OrderBy(u => u.Nome),

            "email" => filtro.Desc
                ? query.OrderByDescending(u => u.Email)
                : query.OrderBy(u => u.Email),

            "datacriacao" => filtro.Desc
                ? query.OrderByDescending(u => u.DataCriacao)
                : query.OrderBy(u => u.DataCriacao),

            "ativo" => filtro.Desc
                ? query.OrderByDescending(u => u.Ativo)
                : query.OrderBy(u => u.Ativo),

            _ => query.OrderBy(u => u.Id)
        };
    }
}
