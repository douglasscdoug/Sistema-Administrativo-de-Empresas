using FluentValidation;

namespace SistemaEmpresas.Application.Validators;

public static class SenhaValidator
{
    public static IRuleBuilderOptions<T, string> SenhaForte<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Senha é obrigatória.")
            .MinimumLength(8).WithMessage("A senha deve ter no mínimo 8 caracteres.")
            .Matches(@"[A-Za-z]").WithMessage("A senha deve conter pelo menos uma letra.")
            .Matches(@"\d").WithMessage("A senha deve conter pelo menos um número.")
            .Must(s => !SenhasComuns.Contains(s.ToLower())).WithMessage("Essa senha é muito comum");
    }

    private static readonly string[] SenhasComuns =
    {
        "senha1234", "passw0rd", "senha123", "senha4321", "senha123"
    };
}
