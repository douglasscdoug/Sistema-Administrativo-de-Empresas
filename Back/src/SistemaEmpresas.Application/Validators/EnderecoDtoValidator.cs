using FluentValidation;
using SistemaEmpresas.Application.Common.Utils;
using SistemaEmpresas.Application.DTOs;

namespace SistemaEmpresas.Application.Validators;

public class EnderecoDtoValidator : AbstractValidator<EnderecoRequestDto>
{
    public EnderecoDtoValidator()
    {
        RuleFor(x => x.Cep)
            .NotEmpty().WithMessage("CEP é obrigatório.")
            .Must(cep =>
            {
                var limpo = StringUtils.SomenteNumeros(cep);
                return !string.IsNullOrEmpty(limpo) && limpo.Length == 8;
            }).WithMessage("CEP inválido.");
    }
}