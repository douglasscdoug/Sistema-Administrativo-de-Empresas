using FluentValidation;
using SistemaEmpresas.Application.DTOs;

namespace SistemaEmpresas.Application.Validators;

public class UsuarioDtoValidator : AbstractValidator<UsuarioRequestDto>
{
    public UsuarioDtoValidator()
    {
        RuleFor(x => x.Senha).SenhaForte();
    }
}
