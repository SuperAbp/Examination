using System.Linq;
using FluentValidation;
using Microsoft.Extensions.Localization;
using SuperAbp.Exam.Localization;
using SuperAbp.Exam.QuestionManagement.Questions;

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

        // RuleFor(q => q)
        //     .Must(q => ValidateCondition(q.QuestionType, q.Options.Count(a => a.Right)))
        //     .WithMessage("");
    }

    private bool ValidateCondition(QuestionType questionType, int count)
    {
        return questionType switch
        {
            QuestionType.Judge => count == 1,
            QuestionType.SingleSelect => count == 1,
            QuestionType.MultiSelect => count > 1,
            _ => true
        };
    }
}