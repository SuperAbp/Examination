using FluentValidation;
using Microsoft.Extensions.Localization;
using SuperAbp.Exam.Localization;

namespace SuperAbp.Exam.Admin.QuestionManagement.Questions;

public class QuestionCreateDtoValidator : AbstractValidator<QuestionCreateDto>
{
    public QuestionCreateDtoValidator(IStringLocalizer<ExamResource> local)
    {
        Include(new QuestionCreateOrUpdateDtoBaseValidator(local));
        RuleFor(q => q.QuestionType)
            .NotNull()
            .WithMessage(local["The {0} field is required.", "{PropertyName}"])
            .IsInEnum()
            .WithMessage(local["The field {0} is invalid.", "{PropertyName}"]);
    }
}