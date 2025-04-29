using System;
using FluentValidation;
using Microsoft.Extensions.Localization;
using SuperAbp.Exam.Localization;

namespace SuperAbp.Exam.Admin.PaperManagement.PaperQuestionRules;

public class GetPaperQuestionRulesInputValidator : AbstractValidator<GetPaperQuestionRulesInput>
{
    public GetPaperQuestionRulesInputValidator(IStringLocalizer<ExamResource> local)
    {
        RuleFor(q => q.PaperId)
            .NotNull()
            .NotEmpty()
            .WithMessage(local["The {0} field is required.", "{PropertyName}"])
            .NotEqual(Guid.Empty)
            .WithMessage(local["The field {0} is invalid.", "{PropertyName}"]);
    }
}