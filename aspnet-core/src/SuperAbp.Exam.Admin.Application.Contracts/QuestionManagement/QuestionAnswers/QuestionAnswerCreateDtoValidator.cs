using System;
using FluentValidation;
using Microsoft.Extensions.Localization;
using SuperAbp.Exam.Localization;

namespace SuperAbp.Exam.Admin.QuestionManagement.QuestionAnswers;

public class QuestionAnswerCreateDtoValidator : AbstractValidator<QuestionAnswerCreateDto>
{
    public QuestionAnswerCreateDtoValidator(IStringLocalizer<ExamResource> local)
    {
        Include(new QuestionAnswerCreateOrUpdateDtoBaseValidator(local));
        RuleFor(q => q.QuestionId)
            .NotNull()
            .WithMessage(local["The {0} field is required.", "{PropertyName}"])
            .NotEqual(Guid.Empty)
            .WithMessage(local["The field {0} is invalid.", "{PropertyName}"]);
    }
}