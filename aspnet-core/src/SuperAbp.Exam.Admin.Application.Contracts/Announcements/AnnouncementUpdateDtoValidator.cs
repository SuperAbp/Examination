using FluentValidation;
using Microsoft.Extensions.Localization;
using SuperAbp.Exam.Localization;

namespace SuperAbp.Exam.Admin.Announcements;

public class AnnouncementUpdateDtoValidator : AbstractValidator<AnnouncementUpdateDto>
{
    public AnnouncementUpdateDtoValidator(IStringLocalizer<ExamResource> local)
    {
        Include(new AnnouncementCreateOrUpdateDtoBaseValidator(local));
    }
}