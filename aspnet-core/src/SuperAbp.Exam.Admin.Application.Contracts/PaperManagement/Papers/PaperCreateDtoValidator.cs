using FluentValidation;
using Microsoft.Extensions.Localization;
using SuperAbp.Exam.Localization;

namespace SuperAbp.Exam.Admin.PaperManagement.Papers;

public class PaperCreateDtoValidator : AbstractValidator<PaperCreateDto>
{
    public PaperCreateDtoValidator(IStringLocalizer<ExamResource> local)
    {
        Include(new PaperCreateOrUpdateDtoBaseValidator(local));
    }
}