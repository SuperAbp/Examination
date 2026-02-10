using FluentValidation;
using Microsoft.Extensions.Localization;
using SuperAbp.Exam.Localization;
using Volo.Abp.Timing;

namespace SuperAbp.Exam.Admin.Announcements;

public class AnnouncementUpdateDtoValidator : AbstractValidator<AnnouncementUpdateDto>
{
    public AnnouncementUpdateDtoValidator(IStringLocalizer<ExamResource> local, IClock clock)
    {
        Include(new AnnouncementCreateOrUpdateDtoBaseValidator(local, clock));
    }
}