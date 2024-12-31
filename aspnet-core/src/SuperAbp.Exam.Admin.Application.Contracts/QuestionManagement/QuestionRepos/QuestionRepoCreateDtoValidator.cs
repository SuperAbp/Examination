using FluentValidation;
using Microsoft.Extensions.Localization;
using SuperAbp.Exam.Localization;

namespace SuperAbp.Exam.Admin.QuestionManagement.QuestionRepos;

public class QuestionRepoCreateDtoValidator : AbstractValidator<QuestionRepoCreateDto>
{
    public QuestionRepoCreateDtoValidator(IStringLocalizer<ExamResource> local)
    {
        Include(new QuestionRepoCreateOrUpdateDtoBaseValidator(local));
    }
}