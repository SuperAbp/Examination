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
            .GreaterThanOrEqualTo(0);
    }
}