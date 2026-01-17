using SistemaEmpresas.Application.DTOs;
using FluentValidation;

namespace SistemaEmpresas.Application.Validators;

public class EmpresaRequestDtoValidator : AbstractValidator<EmpresaRequestDto>
{
    public EmpresaRequestDtoValidator()
    {
        RuleFor(x => x.RazaoSocial)
            .NotEmpty().WithMessage("Razão Social é obrigatória.")
            .MaximumLength(100);

        RuleFor(x => x.Cnpj)
            .NotEmpty().WithMessage("CNPJ é obrigatório.")
            .Must(CnpjValidator.IsValid)
            .WithMessage("CNPJ inválido.");

        RuleFor(x => x.Endereco).NotNull().WithMessage("Endereço é obrigatório");

        RuleFor(x => x.Contato).NotNull().WithMessage("Contato é obrigatório.");
    }
}