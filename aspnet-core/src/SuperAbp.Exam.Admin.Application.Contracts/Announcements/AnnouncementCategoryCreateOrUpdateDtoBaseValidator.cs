using FluentValidation;
using Microsoft.Extensions.Localization;
using SuperAbp.Exam.Announcements;
using SuperAbp.Exam.Localization;

namespace SuperAbp.Exam.Admin.Announcements;

public class AnnouncementCategoryCreateOrUpdateDtoBaseValidator : AbstractValidator<AnnouncementCategoryCreateOrUpdateDtoBase>
{
    public AnnouncementCategoryCreateOrUpdateDtoBaseValidator(IStringLocalizer<ExamResource> local)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(local["The {0} field is required.", "{PropertyName}"])
            .MaximumLength(AnnouncementCategoryConsts.MaxNameLength)
            .WithMessage(local["The {0} field must be less than {1} characters.", "{PropertyName}", AnnouncementCategoryConsts.MaxNameLength]);

        RuleFor(x => x.Remark)
            .MaximumLength(AnnouncementCategoryConsts.MaxRemarkLength)
            .WithMessage(local["The {0} field must be less than {1} characters.", "{PropertyName}", AnnouncementCategoryConsts.MaxRemarkLength])
            .When(x => x.Remark != null);
    }
}
