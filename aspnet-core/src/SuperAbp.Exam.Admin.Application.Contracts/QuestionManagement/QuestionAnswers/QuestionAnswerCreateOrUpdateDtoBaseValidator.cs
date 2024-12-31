using FluentValidation;
using Microsoft.Extensions.Localization;
using SuperAbp.Exam.Localization;

namespace SuperAbp.Exam.Admin.QuestionManagement.QuestionAnswers;

public class QuestionAnswerCreateOrUpdateDtoBaseValidator : AbstractValidator<QuestionAnswerCreateOrUpdateDtoBase>
{
    public QuestionAnswerCreateOrUpdateDtoBaseValidator(IStringLocalizer<ExamResource> local)
    {
        RuleFor(q => q.Content)
            .NotNull()
            .NotEmpty()
            .WithMessage(local["The {0} field is required.", "{PropertyName}"]);
        RuleFor(q => q.Sort)
            .GreaterThanOrEqualTo(0);
    }
}