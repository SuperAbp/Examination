using FluentValidation;
using Microsoft.Extensions.Localization;
using SuperAbp.Exam.Localization;

namespace SuperAbp.Exam.Admin.ExamManagement.Exams;

public class ExamCreateDtoValidator : AbstractValidator<ExamCreateDto>
{
    public ExamCreateDtoValidator(IStringLocalizer<ExamResource> local)
    {
        Include(new ExamCreateOrUpdateDtoBaseValidator(local));
    }
}