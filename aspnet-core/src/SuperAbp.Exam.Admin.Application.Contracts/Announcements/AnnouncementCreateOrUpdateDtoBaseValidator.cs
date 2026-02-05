using System;
using FluentValidation;
using Microsoft.Extensions.Localization;
using SuperAbp.Exam.Announcements;
using SuperAbp.Exam.Localization;

namespace SuperAbp.Exam.Admin.Announcements;

public class AnnouncementCreateOrUpdateDtoBaseValidator : AbstractValidator<AnnouncementCreateOrUpdateDtoBase>
{
    public AnnouncementCreateOrUpdateDtoBaseValidator(IStringLocalizer<ExamResource> local)
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(local["The {0} field is required.", "{PropertyName}"])
            .MaximumLength(AnnouncementConsts.MaxTitleLength)
            .WithMessage(local["The {0} field must be less than {1} characters.", "{PropertyName}", AnnouncementConsts.MaxTitleLength]);

        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage(local["The {0} field is required.", "{PropertyName}"])
            .MaximumLength(AnnouncementConsts.MaxContentLength)
            .WithMessage(local["The {0} field must be less than {1} characters.", "{PropertyName}", AnnouncementConsts.MaxContentLength]);

        RuleFor(x => x.ExpirationTime)
            .Must((dto, expirationTime) => !expirationTime.HasValue || expirationTime.Value > DateTime.Now)
            .WithMessage(local["Expiration time must be in the future."])
            .When(x => x.ExpirationTime.HasValue);
    }
}
