using FluentValidation;
using SistemaEmpresas.Application.Common.Utils;
using SistemaEmpresas.Application.DTOs;

namespace SistemaEmpresas.Application.Validators;

public class ContatoDtoValidator : AbstractValidator<ContatoRequestDto>
{
    public ContatoDtoValidator()
    {
        RuleFor(x => x.Telefone)
            .NotEmpty().WithMessage("Telefone é obrigatório.")
            .Must(tel =>
            {
                var limpo = StringUtils.SomenteNumeros(tel);
                return !string.IsNullOrEmpty(limpo) && (limpo.Length == 10 || limpo.Length == 11);
            })
            .WithMessage("Telefone inválido");
    }
}