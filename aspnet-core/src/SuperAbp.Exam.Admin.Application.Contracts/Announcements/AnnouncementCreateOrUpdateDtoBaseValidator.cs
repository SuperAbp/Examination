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

        RuleFor(x => x.PublishTime)
            .Must((dto, publishTime) => clock.ConvertToUtc(publishTime.Value) >= clock.Now)
            .WithMessage(local["Publish time must be in the future or now."])
            .When(x => x.PublishTime.HasValue);

        RuleFor(x => x.ExpirationTime)
            .Must((dto, expirationTime) => clock.ConvertToUtc(expirationTime.Value) > clock.Now)
            .WithMessage(local["Expiration time must be in the future."])
            .When(x => x.ExpirationTime.HasValue);

        RuleFor(x => x)
            .Must(dto => clock.ConvertToUtc(dto.PublishTime.Value) < clock.ConvertToUtc(dto.ExpirationTime.Value))
            .WithMessage(local["Expiration time must be after publish time."])
            .When(x => x.PublishTime.HasValue && x.ExpirationTime.HasValue);
    }
}