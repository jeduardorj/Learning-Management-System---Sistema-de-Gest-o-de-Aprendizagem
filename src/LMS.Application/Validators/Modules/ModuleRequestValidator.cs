using FluentValidation;
using LMS.Application.DTOs.Modules;

namespace LMS.Application.Validators.Modules;

public class ModuleRequestValidator : AbstractValidator<ModuleRequestDto>
{
    public ModuleRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Título é obrigatório.")
            .MinimumLength(3).WithMessage("Título deve ter ao menos 3 caracteres.")
            .MaximumLength(200).WithMessage("Título deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Order)
            .GreaterThan(0).WithMessage("Ordem deve ser maior que zero.");
    }
}
