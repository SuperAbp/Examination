using System;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using SuperAbp.Exam.Announcements;
using SuperAbp.Exam.Localization;
using Volo.Abp.Timing;

namespace SuperAbp.Exam.Admin.Announcements;

public class AnnouncementCreateOrUpdateDtoBaseValidator : AbstractValidator<AnnouncementCreateOrUpdateDtoBase>
{
    public AnnouncementCreateOrUpdateDtoBaseValidator(IStringLocalizer<ExamResource> local, IClock clock)
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

        RuleFor(x => x.ScheduledPublishTime)
            .Must((dto, scheduledPublishTime) => clock.ConvertToUtc(scheduledPublishTime.Value) >= clock.Now)
            .WithMessage(local["Publish time must be in the future or now."])
            .When(x => x.ScheduledPublishTime.HasValue);

        RuleFor(x => x.ScheduledExpirationTime)
            .Must((dto, scheduledExpirationTime) => clock.ConvertToUtc(scheduledExpirationTime.Value) > clock.Now)
            .WithMessage(local["Expiration time must be in the future."])
            .When(x => x.ScheduledExpirationTime.HasValue);

        RuleFor(x => x)
            .Must(dto => clock.ConvertToUtc(dto.ScheduledPublishTime.Value) < clock.ConvertToUtc(dto.ScheduledExpirationTime.Value))
            .WithMessage(local["Expiration time must be after publish time."])
            .When(x => x.ScheduledPublishTime.HasValue && x.ScheduledExpirationTime.HasValue);
    }
}