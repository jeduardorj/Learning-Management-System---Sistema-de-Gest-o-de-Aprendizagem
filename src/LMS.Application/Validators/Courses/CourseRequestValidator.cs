using FluentValidation;
using LMS.Application.DTOs.Courses;

namespace LMS.Application.Validators.Courses;

public class CourseRequestValidator : AbstractValidator<CourseRequestDto>
{
    public CourseRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Título é obrigatório.")
            .MinimumLength(3).WithMessage("Título deve ter ao menos 3 caracteres.")
            .MaximumLength(200).WithMessage("Título deve ter no máximo 200 caracteres.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Descrição é obrigatória.")
            .MinimumLength(10).WithMessage("Descrição deve ter ao menos 10 caracteres.")
            .MaximumLength(2000).WithMessage("Descrição deve ter no máximo 2000 caracteres.");
    }
}
