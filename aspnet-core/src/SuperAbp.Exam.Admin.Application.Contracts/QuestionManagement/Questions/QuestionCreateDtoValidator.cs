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

        // RuleFor(q => q)
        //     .Must(q => ValidateCondition(q.QuestionType, q.Options.Count(a => a.Right)))
        //     .WithMessage("");
    }

    private bool ValidateCondition(int questionType, int count)
    {
        switch (QuestionType.FromValue(questionType).Name)
        {
            case nameof(QuestionType.SingleSelect):
            case nameof(QuestionType.Judge):
                return count == 1;

            case nameof(QuestionType.MultiSelect):
                return count > 1;

            default:
                return true;
        }
    }
}