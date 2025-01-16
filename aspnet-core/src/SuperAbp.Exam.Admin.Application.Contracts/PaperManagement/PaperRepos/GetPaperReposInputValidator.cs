using System;
using FluentValidation;
using Microsoft.Extensions.Localization;
using SuperAbp.Exam.Localization;

namespace SuperAbp.Exam.Admin.PaperManagement.PaperRepos;

public class GetPaperReposInputValidator : AbstractValidator<GetPaperReposInput>
{
    public GetPaperReposInputValidator(IStringLocalizer<ExamResource> local)
    {
        RuleFor(q => q.PaperId)
            .NotNull()
            .NotEmpty()
            .WithMessage(local["The {0} field is required.", "{PropertyName}"])
            .NotEqual(Guid.Empty)
            .WithMessage(local["The field {0} is invalid.", "{PropertyName}"]);
    }
}