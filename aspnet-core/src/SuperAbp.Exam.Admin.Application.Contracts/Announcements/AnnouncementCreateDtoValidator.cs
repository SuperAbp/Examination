using FluentValidation;
using Microsoft.Extensions.Localization;
using SuperAbp.Exam.Localization;

namespace SuperAbp.Exam.Admin.Announcements;

public class AnnouncementCreateDtoValidator : AbstractValidator<AnnouncementCreateDto>
{
    public AnnouncementCreateDtoValidator(IStringLocalizer<ExamResource> local)
    {
        Include(new AnnouncementCreateOrUpdateDtoBaseValidator(local));
    }
}