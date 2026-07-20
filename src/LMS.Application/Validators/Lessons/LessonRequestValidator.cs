using FluentValidation;
using LMS.Application.DTOs.Lessons;

namespace LMS.Application.Validators.Lessons;

public class LessonRequestValidator : AbstractValidator<LessonRequestDto>
{
    public LessonRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Título é obrigatório.")
            .MinimumLength(3).WithMessage("Título deve ter ao menos 3 caracteres.")
            .MaximumLength(200).WithMessage("Título deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Conteúdo é obrigatório.")
            .MinimumLength(10).WithMessage("Conteúdo deve ter ao menos 10 caracteres.")
            .MaximumLength(5000).WithMessage("Conteúdo deve ter no máximo 5000 caracteres.");

        RuleFor(x => x.Order)
            .GreaterThan(0).WithMessage("Ordem deve ser maior que zero.");
    }
}
