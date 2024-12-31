using FluentValidation;
using Microsoft.Extensions.Localization;
using SuperAbp.Exam.Localization;

namespace SuperAbp.Exam.Admin.PaperManagement.Papers;

public class PaperUpdateDtoValidator : AbstractValidator<PaperUpdateDto>
{
    public PaperUpdateDtoValidator(IStringLocalizer<ExamResource> local)
    {
        Include(new PaperCreateOrUpdateDtoBaseValidator(local));
    }
}