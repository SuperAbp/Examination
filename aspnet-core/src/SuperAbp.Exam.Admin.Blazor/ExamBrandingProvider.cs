using Microsoft.Extensions.Localization;
using SuperAbp.Exam.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace SuperAbp.Exam.Blazor;

[Dependency(ReplaceServices = true)]
public class ExamBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<ExamResource> _localizer;

    public ExamBrandingProvider(IStringLocalizer<ExamResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
