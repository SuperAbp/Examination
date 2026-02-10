using FluentValidation;
using Microsoft.Extensions.Localization;
using SuperAbp.Exam.Localization;

namespace SuperAbp.Exam.Admin.Announcements;

public class AnnouncementCategoryCreateDtoValidator : AbstractValidator<AnnouncementCategoryCreateDto>
{
    public AnnouncementCategoryCreateDtoValidator(IStringLocalizer<ExamResource> local)
    {
        Include(new AnnouncementCategoryCreateOrUpdateDtoBaseValidator(local));
    }
}