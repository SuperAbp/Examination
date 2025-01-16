using System;
using FluentValidation;
using Microsoft.Extensions.Localization;
using SuperAbp.Exam.Localization;

namespace SuperAbp.Exam.Admin.ExamManagement.Exams;

public class ExamCreateOrUpdateDtoBaseValidator : AbstractValidator<ExamCreateOrUpdateDtoBase>
{
    public ExamCreateOrUpdateDtoBaseValidator(IStringLocalizer<ExamResource> local)
    {
        RuleFor(q => q.PaperId)
            .NotNull()
            .WithMessage(local["The {0} field is required.", "{PropertyName}"])
            .NotEqual(Guid.Empty)
            .WithMessage(local["The field {0} is invalid.", "{PropertyName}"]);
        RuleFor(q => q.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage(local["The {0} field is required.", "{PropertyName}"]);
        RuleFor(q => q.Score)
            .GreaterThanOrEqualTo(0);
        RuleFor(q => q.PassingScore)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(q => q.Score);

        When(q => q.StartTime.HasValue && q.EndTime.HasValue, () =>
        {
            RuleFor(q => q.StartTime)
                .LessThan(q => q.EndTime)
                .When(q => q.StartTime.HasValue);

            RuleFor(q => q.EndTime)
                .GreaterThan(q => q.StartTime)
                .When(q => q.EndTime.HasValue);
        });
    }
}