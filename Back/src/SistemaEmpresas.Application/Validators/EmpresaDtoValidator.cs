using SistemaEmpresas.Application.DTOs;
using FluentValidation;
using SistemaEmpresas.Application.Common.Utils;

namespace SistemaEmpresas.Application.Validators;

public class EmpresaDtoValidator : AbstractValidator<EmpresaRequestDto>
{
    public EmpresaDtoValidator()
    {
        RuleFor(x => x.RazaoSocial)
            .NotEmpty().WithMessage("Razão Social é obrigatória.")
            .MaximumLength(100);

        RuleFor(x => x.Cnpj)
            .NotEmpty().WithMessage("CNPJ é obrigatório.")
            .Must(cnpj =>
            {
                var limpo = StringUtils.SomenteNumeros(cnpj);
                return CnpjValidator.IsValid(limpo);
            })
            .WithMessage("CNPJ inválido.");

        RuleFor(x => x.Endereco)
            .NotNull().WithMessage("Endereço é obrigatório")
            .SetValidator(new EnderecoDtoValidator());

        RuleFor(x => x.Contato)
            .NotNull().WithMessage("Contato é obrigatório.")
            .SetValidator(new ContatoDtoValidator());
    }
}