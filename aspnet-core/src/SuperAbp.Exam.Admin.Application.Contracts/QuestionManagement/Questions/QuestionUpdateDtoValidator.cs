using FluentValidation;
using Microsoft.Extensions.Localization;
using SuperAbp.Exam.Localization;

namespace SuperAbp.Exam.Admin.QuestionManagement.Questions;

public class QuestionUpdateDtoValidator : AbstractValidator<QuestionUpdateDto>
{
    public QuestionUpdateDtoValidator(IStringLocalizer<ExamResource> local)
    {
        Include(new QuestionCreateOrUpdateDtoBaseValidator(local));
    }
}