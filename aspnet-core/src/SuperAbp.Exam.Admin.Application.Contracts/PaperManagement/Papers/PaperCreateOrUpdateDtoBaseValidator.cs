using System.Linq;
using FluentValidation;
using Microsoft.Extensions.Localization;
using SuperAbp.Exam.Localization;

namespace SuperAbp.Exam.Admin.PaperManagement.Papers;

public class PaperCreateOrUpdateDtoBaseValidator : AbstractValidator<PaperCreateOrUpdateDtoBase>
{
    public PaperCreateOrUpdateDtoBaseValidator(IStringLocalizer<ExamResource> local)
    {
        RuleFor(q => q.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage(local["The {0} field is required.", "{PropertyName}"]);

        RuleFor(q => q.Score)
            .Must((a, score) => score == a.PaperQuestionRules.Sum(r => r.SingleScore * r.SingleCount + r.MultiScore * r.MultiCount + r.JudgeScore * r.JudgeCount + r.BlankScore * r.BlankCount))
            .WithMessage(local["The field {0} is invalid.", "{PropertyName}"]);

        RuleFor(q => q.PaperQuestionRules)
            .NotNull()
            .NotEmpty()
            .WithMessage(local["The {0} field is required.", "{PropertyName}"]);
    }
}