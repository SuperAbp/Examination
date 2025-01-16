using System.Linq;
using FluentValidation;
using Microsoft.Extensions.Localization;
using SuperAbp.Exam.Localization;

namespace SuperAbp.Exam.Admin.PaperManagement.Papers;

public class PaperUpdateDtoValidator : AbstractValidator<PaperUpdateDto>
{
    public PaperUpdateDtoValidator(IStringLocalizer<ExamResource> local)
    {
        Include(new PaperCreateOrUpdateDtoBaseValidator(local));

        RuleFor(q => q.Repositories)
            .NotNull()
            .NotEmpty()
            .WithMessage(local["The {0} field is required.", "{PropertyName}"]);

        RuleFor(q => q.Score)
            .Must((a, score) => score == a.Repositories.Sum(r => r.SingleScore + r.MultiScore + r.JudgeScore + r.BlankScore))
            .WithMessage(local["The field {0} is invalid.", "{PropertyName}"]);
    }
}