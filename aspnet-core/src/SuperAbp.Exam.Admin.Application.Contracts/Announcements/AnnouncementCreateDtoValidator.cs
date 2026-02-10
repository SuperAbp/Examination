using FluentValidation;
using Microsoft.Extensions.Localization;
using SuperAbp.Exam.Localization;
using Volo.Abp.Timing;

namespace SuperAbp.Exam.Admin.Announcements;

public class AnnouncementCreateDtoValidator : AbstractValidator<AnnouncementCreateDto>
{
    public AnnouncementCreateDtoValidator(IStringLocalizer<ExamResource> local, IClock clock)
    {
        Include(new AnnouncementCreateOrUpdateDtoBaseValidator(local, clock));
    }
}