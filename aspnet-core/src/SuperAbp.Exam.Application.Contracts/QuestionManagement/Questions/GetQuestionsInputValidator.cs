using FluentValidation;
using Microsoft.Extensions.Localization;
using SuperAbp.Exam.Localization;
using System;

namespace SuperAbp.Exam.QuestionManagement.Questions;

public class GetQuestionsInputValidator : AbstractValidator<GetQuestionsInput>
{
    public GetQuestionsInputValidator(IStringLocalizer<ExamResource> local)
    {
        RuleFor(q => q.QuestionBankId)
            .NotNull()
            .NotEmpty()
            .WithMessage(local["The {0} field is required.", "{PropertyName}"])
            .NotEqual(Guid.Empty)
            .WithMessage(local["The field {0} is invalid.", "{PropertyName}"]);
    }
}