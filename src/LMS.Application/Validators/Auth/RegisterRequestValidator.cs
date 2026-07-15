using FluentValidation;
using LMS.Application.DTOs.Auth;

namespace LMS.Application.Validators.Auth;

public class RegisterRequestValidator : AbstractValidator<RegisterRequestDto>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório.")
            .MinimumLength(3).WithMessage("Nome deve ter ao menos 3 caracteres.")
            .MaximumLength(200).WithMessage("Nome deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-mail é obrigatório.")
            .EmailAddress().WithMessage("E-mail inválido.")
            .MaximumLength(300).WithMessage("E-mail deve ter no máximo 300 caracteres.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Senha é obrigatória.")
            .MinimumLength(8).WithMessage("Senha deve ter ao menos 8 caracteres.")
            .Matches("[A-Z]").WithMessage("Senha deve ter ao menos uma letra maiúscula.")
            .Matches("[a-z]").WithMessage("Senha deve ter ao menos uma letra minúscula.")
            .Matches("[0-9]").WithMessage("Senha deve ter ao menos um número.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Senha deve ter ao menos um caractere especial.");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Confirmação de senha é obrigatória.")
            .Equal(x => x.Password).WithMessage("As senhas não conferem.");
    }
}
