using FluentValidation;
using Microsoft.Extensions.Localization;
using SuperAbp.Exam.Localization;

namespace SuperAbp.Exam.Admin.QuestionManagement.QuestionBanks;

public class QuestionBankUpdateDtoValidator : AbstractValidator<QuestionBankUpdateDto>
{
    public QuestionBankUpdateDtoValidator(IStringLocalizer<ExamResource> local)
    {
        Include(new QuestionBankCreateOrUpdateDtoBaseValidator(local));
    }
}