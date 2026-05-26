using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SistemaEmpresas.Application.Common.Utils;
using SistemaEmpresas.Application.DTOs;
using SistemaEmpresas.Application.Exceptions;
using SistemaEmpresas.Application.Filters;
using SistemaEmpresas.Application.Interfaces;
using SistemaEmpresas.Domain.Entities;
using SistemaEmpresas.Infrastructure.Repositories;

namespace SistemaEmpresas.Application.Services;

public class EmpresaService : IEmpresaService
{
    private readonly IMapper _mapper;
    private readonly EmpresaRepository _empresaRepository;
    private readonly ILogger<EmpresaService> _logger;

    public EmpresaService(
        IMapper mapper,
        EmpresaRepository empresaRepository,
        ILogger<EmpresaService> logger
    )
    {
        _mapper = mapper;
        _empresaRepository = empresaRepository;
        _logger = logger;
    }

    public async Task<PagedResult<EmpresaResponseDto>> Filtrar(EmpresaFiltroDto filtro)
    {
        _logger.LogInformation(
            "Iniciando filtro de empresas. Page={Page}, PageSize={PageSize}, Serch={Search}",
            filtro.Page,
            filtro.PageSize,
            filtro.Search
        );

        var query = _empresaRepository.Query();

        //Busca global
        if (!string.IsNullOrWhiteSpace(filtro.Search))
        {
            query = query.Where(e =>
                e.RazaoSocial.Contains(filtro.Search) ||
                e.Cnpj.Contains(filtro.Search));
        }

        //filtros específicos
        if (!string.IsNullOrWhiteSpace(filtro.RazaoSocial))
        {
            var termo = filtro.RazaoSocial.Trim().ToLower();

            query = query.Where(e => e.RazaoSocial.ToLower().Contains(termo));
        }

        if (!string.IsNullOrWhiteSpace(filtro.Cnpj))
            query = query.Where(e => e.Cnpj.Contains(filtro.Cnpj));

        if (filtro.Ativo.HasValue)
            query = query.Where(e => e.Ativo == filtro.Ativo.Value);

        var total = await query.CountAsync();

        query = ApplyOrdering(query, filtro);

        //Paginação
        var data = await query
            .Skip((filtro.Page - 1) * filtro.PageSize)
            .Take(filtro.PageSize)
            .ProjectTo<EmpresaResponseDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        _logger.LogInformation(
            "Filtro de empresas finalizado. Total={Total}, Retornado={Retornados}",
            total,
            data.Count
        );

        return new PagedResult<EmpresaResponseDto>
        {
            Data = data,
            Total = total,
            Page = filtro.Page,
            PageSize = filtro.PageSize
        };
    }

    public async Task<EmpresaResponseDto?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Buscando empresa {EmpresaId}", id);

        var empresa = await _empresaRepository.GetByIdAsync(id);

        if (empresa == null)
        {
            _logger.LogWarning("Empresa {EmpresaId} não encontrada", id);
            return null;
        }

        _logger.LogInformation("Empresa {EmpresaId} encontrada com sucesso", id);

        return _mapper.Map<EmpresaResponseDto>(empresa);
    }

    public async Task<EmpresaResponseDto> AddAsync(EmpresaRequestDto dto)
    {
        _logger.LogInformation(
            "Iniciando cadastro da empresa {RazaoSocial}",
            dto.RazaoSocial
        );

        dto.Cnpj = StringUtils.SomenteNumeros(dto.Cnpj);
        dto.Endereco.Cep = StringUtils.SomenteNumeros(dto.Endereco.Cep);
        dto.Contato.Telefone = StringUtils.SomenteNumeros(dto.Contato.Telefone);

        var empresaExistente = await _empresaRepository.GetByCnpjAsync(dto.Cnpj);
        if (empresaExistente != null)
        {
            _logger.LogWarning(
                "Tentativa de cadastro com CNPJ já existente {Cnpj}",
                dto.Cnpj
            );

            throw new BusinessException("Cnpj", "Já existe uma empresa cadastrada com esse CNPJ.");
        }

        var empresa = _mapper.Map<Empresa>(dto);
        empresa.Ativo = true;

        await _empresaRepository.AddAsync(empresa);

        var sucess = await _empresaRepository.SaveChangesAsync();
        if (!sucess)
        {
            _logger.LogError(
                "Erro ao salvar empresa {RazaoSocial}",
                dto.RazaoSocial
            );

            throw new BusinessException("Erro", "Ocorreu um erro ao salvar a empresa.");
        }

        _logger.LogInformation(
            "Empresa {EmpresaId} cadastrada com sucesso",
            empresa.Id
        );

        return _mapper.Map<EmpresaResponseDto>(empresa);
    }

    public async Task<EmpresaResponseDto?> UpdateAsync(Guid id, EmpresaRequestDto dto)
    {
        _logger.LogInformation(
            "Iniciando atualização da empresa {EmpresaId}",
            id
        );

        var empresa = await _empresaRepository.GetByIdForUpdateAsync(id);
        if (empresa == null)
        {
            _logger.LogWarning(
                "Tentativa de atualização de empresa inexistente {EmpresaId}",
                id
            );

            return null;
        }

        dto.Cnpj = StringUtils.SomenteNumeros(dto.Cnpj);
        dto.Endereco.Cep = StringUtils.SomenteNumeros(dto.Endereco.Cep);
        dto.Contato.Telefone = StringUtils.SomenteNumeros(dto.Contato.Telefone);

        if (empresa.Cnpj != dto.Cnpj)
        {
            var empresaExistente = await _empresaRepository.GetByCnpjAsync(dto.Cnpj);
            if (empresaExistente != null)
            {
                _logger.LogWarning(
                    "Tentativa de atualizar empresa {EmpresaId} com CNPJ duplicado",
                    id
                );

                throw new BusinessException("Cnpj", "Já existe uma empresa cadastrada com esse CNPJ.");
            }
        }

        //Empresa 
        empresa.RazaoSocial = dto.RazaoSocial;
        empresa.Cnpj = dto.Cnpj;
        empresa.Ativo = dto.Ativo;

        //Endereço
        empresa.Endereco.Logradouro = dto.Endereco.Logradouro;
        empresa.Endereco.Numero = dto.Endereco.Numero;
        empresa.Endereco.Complemento = dto.Endereco.Complemento;
        empresa.Endereco.Bairro = dto.Endereco.Bairro;
        empresa.Endereco.Cidade = dto.Endereco.Cidade;
        empresa.Endereco.Estado = dto.Endereco.Estado;
        empresa.Endereco.Cep = dto.Endereco.Cep;

        //Contato
        empresa.Contato.Nome = dto.Contato.Nome;
        empresa.Contato.Email = dto.Contato.Email;
        empresa.Contato.Telefone = dto.Contato.Telefone;

        var sucess = await _empresaRepository.SaveChangesAsync();
        if (!sucess)
        {
            _logger.LogError(
                "Erro ao salvar empresa {EmpresaId}",
                id
            );

            throw new BusinessException("Erro", "Ocorreu um erro ao atualizar a empresa.");
        }

        _logger.LogInformation(
            "Empresa {EmpresaId} atualizada com sucesso",
            id
        );

        return _mapper.Map<EmpresaResponseDto>(empresa);
    }

    public async Task<bool> AtivarEmpresaAsync(Guid id)
    {
        _logger.LogInformation(
            "Iniciando ativação de empresa {EmpresaId}",
            id
        );

        var empresa = await _empresaRepository.GetByIdForUpdateAsync(id);

        if (empresa == null)
        {
            _logger.LogWarning(
                "Tentativa de ativação de empresa inexistente {EmpresaId}",
                id
            );

            return false;
        }

        empresa.Ativo = true;

        var success = await _empresaRepository.SaveChangesAsync();

        if (success)
        {
            _logger.LogInformation(
                "Empresa {EmpresaId} ativada com sucesso",

                id
            );
        }

        return success;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        _logger.LogInformation(
            "Iniciando exclusão lógica da empresa {EmpresaId}",
            id
        );

        var empresa = await _empresaRepository.GetByIdForUpdateAsync(id);

        if (empresa == null)
        {
            _logger.LogWarning(
                "Tentativa de excluir empresa inexistente {EmpresaId}",
                id
            );

            return false;
        }

        empresa.Ativo = false;

        var success = await _empresaRepository.SaveChangesAsync();

        if(success)
        {
            _logger.LogInformation(
                "Empresa {EmpresaId} desativada com sucesso",
                id
            );
        }

        return success;
    }

    private IQueryable<Empresa> ApplyOrdering(IQueryable<Empresa> query, PagedRequest filtro)
    {
        if (string.IsNullOrWhiteSpace(filtro.OrderBy))
            return query.OrderBy(e => e.Id);

        return filtro.OrderBy.ToLower() switch
        {
            "razaosocial" => filtro.Desc
                ? query.OrderByDescending(e => e.RazaoSocial)
                : query.OrderBy(e => e.RazaoSocial),

            "cnpj" => filtro.Desc
                ? query.OrderByDescending(e => e.Cnpj)
                : query.OrderBy(e => e.Cnpj),

            "ativo" => filtro.Desc
                ? query.OrderByDescending(e => e.Ativo)
                : query.OrderBy(e => e.Ativo),

            _ => query.OrderBy(e => e.Id)
        };
    }
}