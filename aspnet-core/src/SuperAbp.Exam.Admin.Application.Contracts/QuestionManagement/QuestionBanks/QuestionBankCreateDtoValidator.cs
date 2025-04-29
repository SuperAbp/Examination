using FluentValidation;
using Microsoft.Extensions.Localization;
using SuperAbp.Exam.Localization;

namespace SuperAbp.Exam.Admin.QuestionManagement.QuestionBanks;

public class QuestionBankCreateDtoValidator : AbstractValidator<QuestionBankCreateDto>
{
    public QuestionBankCreateDtoValidator(IStringLocalizer<ExamResource> local)
    {
        Include(new QuestionBankCreateOrUpdateDtoBaseValidator(local));
    }
}