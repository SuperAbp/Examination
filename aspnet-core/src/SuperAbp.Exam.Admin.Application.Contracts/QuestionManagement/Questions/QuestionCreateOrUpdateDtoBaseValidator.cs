using FluentValidation;
using Microsoft.Extensions.Localization;
using SuperAbp.Exam.Localization;
using System;

namespace SuperAbp.Exam.Admin.QuestionManagement.Questions;

public class QuestionCreateOrUpdateDtoBaseValidator : AbstractValidator<QuestionCreateOrUpdateDtoBase>
{
    public QuestionCreateOrUpdateDtoBaseValidator(IStringLocalizer<ExamResource> local)
    {
        RuleFor(q => q.Content)
            .NotNull()
            .NotEmpty()
            .WithMessage(local["The {0} field is required.", "{PropertyName}"]);
        RuleFor(q => q.QuestionRepositoryId)
            .NotNull()
            .WithMessage(local["The {0} field is required.", "{PropertyName}"])
            .NotEqual(Guid.Empty)
            .WithMessage(local["The field {0} is invalid.", "{PropertyName}"]);
    }
}