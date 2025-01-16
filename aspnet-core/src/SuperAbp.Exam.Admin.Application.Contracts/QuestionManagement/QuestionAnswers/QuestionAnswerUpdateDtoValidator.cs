using FluentValidation;
using Microsoft.Extensions.Localization;
using SuperAbp.Exam.Localization;

namespace SuperAbp.Exam.Admin.QuestionManagement.QuestionAnswers;

public class QuestionAnswerUpdateDtoValidator : AbstractValidator<QuestionAnswerUpdateDto>
{
    public QuestionAnswerUpdateDtoValidator(IStringLocalizer<ExamResource> local)
    {
        Include(new QuestionAnswerCreateOrUpdateDtoBaseValidator(local));
    }
}