using AutoMapper;
using SistemaEmpresas.Application.DTOs;
using SistemaEmpresas.Domain.Entities;

namespace SistemaEmpresas.Application.Helpers;

public class SistemaEmpresasProfile : Profile
{
    public SistemaEmpresasProfile()
    {
        // ===== Contato =====
        CreateMap<Contato, ContatoResponseDto>();
        CreateMap<ContatoRequestDto, Contato>();

        // ===== Endereço =====
        CreateMap<Endereco, EnderecoResponseDto>();
        CreateMap<EnderecoRequestDto, Endereco>();

        // ===== Empresa =====
        CreateMap<Empresa, EmpresaResponseDto>();
        CreateMap<EmpresaRequestDto, Empresa>();
    }
}