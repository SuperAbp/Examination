using FluentValidation;
using Microsoft.Extensions.Localization;
using SuperAbp.Exam.Localization;

namespace SuperAbp.Exam.Admin.QuestionManagement.QuestionRepos;

public class QuestionRepoUpdateDtoValidator : AbstractValidator<QuestionRepoUpdateDto>
{
    public QuestionRepoUpdateDtoValidator(IStringLocalizer<ExamResource> local)
    {
        Include(new QuestionRepoCreateOrUpdateDtoBaseValidator(local));
    }
}