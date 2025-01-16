using FluentValidation;
using Microsoft.Extensions.Localization;
using SuperAbp.Exam.Localization;
using System;

namespace SuperAbp.Exam.Admin.PaperManagement.PaperRepos;

public class PaperRepoCreateDtoValidator : AbstractValidator<PaperRepoCreateDto>
{
    public PaperRepoCreateDtoValidator(IStringLocalizer<ExamResource> local)
    {
        RuleFor(q => q.PaperId)
            .NotNull()
            .WithMessage(local["The {0} field is required.", "{PropertyName}"])
            .NotEqual(Guid.Empty)
            .WithMessage(local["The field {0} is invalid.", "{PropertyName}"]);
        RuleFor(q => q.QuestionRepositoryId)
            .NotNull()
            .WithMessage(local["The {0} field is required.", "{PropertyName}"])
            .NotEqual(Guid.Empty)
            .WithMessage(local["The field {0} is invalid.", "{PropertyName}"]);
    }
}