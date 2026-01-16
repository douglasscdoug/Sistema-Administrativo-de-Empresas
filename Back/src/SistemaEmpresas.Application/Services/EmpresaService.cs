using AutoMapper;
using SistemaEmpresas.Application.DTOs;
using SistemaEmpresas.Application.Interfaces;
using SistemaEmpresas.Domain.Entities;
using SistemaEmpresas.Infrastructure.Repositories;

namespace SistemaEmpresas.Application.Services;

public class EmpresaService : IEmpresaService
{
    private readonly IMapper _mapper;
    private readonly EmpresaRepository _empresaRepository;

    public EmpresaService(IMapper mapper, EmpresaRepository empresaRepository)
    {
        _mapper = mapper;
        _empresaRepository = empresaRepository;
    }

    public async Task<EmpresaResponseDto[]> GetAllAsync()
    {
        var empresas = await _empresaRepository.GetAllAsync();
        return _mapper.Map<EmpresaResponseDto[]>(empresas);
    }

    public async Task<EmpresaResponseDto?> GetByIdAsync(Guid id)
    {
        var empresa = await _empresaRepository.GetByIdAsync(id);

        if(empresa == null)
            return null;

        return _mapper.Map<EmpresaResponseDto>(empresa);
    }

    public async Task<EmpresaResponseDto> AddAsync(EmpresaRequestDto empresaRequestDto)
    {
        var empresa = _mapper.Map<Empresa>(empresaRequestDto);
        empresa.Ativo = true;

        await _empresaRepository.AddAsync(empresa);

        var sucess =  await _empresaRepository.SaveChangesAsync();
        if(!sucess)
            throw new Exception("Ocorreu um erro ao salvar a empresa.");

        return _mapper.Map<EmpresaResponseDto>(empresa);
    }

    public async Task<EmpresaResponseDto?> UpdateAsync(Guid id, EmpresaRequestDto empresaRequestDto)
    {
        var empresa = await _empresaRepository.GetByIdForUpdateAsync(id);
        if(empresa == null)
            return null;

        //Empresa 
        empresa.RazaoSocial = empresaRequestDto.RazaoSocial;
        empresa.Cnpj = empresaRequestDto.Cnpj;
        empresa.Ativo = empresaRequestDto.Ativo;

        //Endereço
        empresa.Endereco.Logradouro = empresaRequestDto.Endereco.Logradouro;
        empresa.Endereco.Numero = empresaRequestDto.Endereco.Numero;
        empresa.Endereco.Complemento = empresaRequestDto.Endereco.Complemento;
        empresa.Endereco.Bairro = empresaRequestDto.Endereco.Bairro;
        empresa.Endereco.Cidade = empresaRequestDto.Endereco.Cidade;
        empresa.Endereco.Estado = empresaRequestDto.Endereco.Estado;
        empresa.Endereco.Cep = empresaRequestDto.Endereco.Cep;

        //Contato
        empresa.Contato.Nome = empresaRequestDto.Contato.Nome;
        empresa.Contato.Email = empresaRequestDto.Contato.Email;
        empresa.Contato.Telefone = empresaRequestDto.Contato.Telefone;

        var sucess = await _empresaRepository.SaveChangesAsync();
        if(!sucess)
            throw new Exception("Ocorreu um erro ao atualizar a empresa.");

        return _mapper.Map<EmpresaResponseDto>(empresa);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var empresa = await _empresaRepository.GetByIdForUpdateAsync(id);

        if(empresa == null)
            return false;

        empresa.Ativo = false;

        return await _empresaRepository.SaveChangesAsync();
    }
}
