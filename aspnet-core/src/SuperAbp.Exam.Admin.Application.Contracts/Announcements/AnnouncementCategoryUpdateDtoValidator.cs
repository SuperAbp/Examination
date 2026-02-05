using FluentValidation;
using Microsoft.Extensions.Localization;
using SuperAbp.Exam.Localization;

namespace SuperAbp.Exam.Admin.Announcements;

public class AnnouncementCategoryUpdateDtoValidator : AbstractValidator<AnnouncementCategoryUpdateDto>
{
    public AnnouncementCategoryUpdateDtoValidator(IStringLocalizer<ExamResource> local)
    {
        Include(new AnnouncementCategoryCreateOrUpdateDtoBaseValidator(local));
    }
}