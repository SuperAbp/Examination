using FluentValidation;
using Microsoft.Extensions.Localization;
using SuperAbp.Exam.Localization;

namespace SuperAbp.Exam.Admin.ExamManagement.Exams;

public class ExamUpdateDtoValidator : AbstractValidator<ExamUpdateDto>
{
    public ExamUpdateDtoValidator(IStringLocalizer<ExamResource> local)
    {
        Include(new ExamCreateOrUpdateDtoBaseValidator(local));
    }
}