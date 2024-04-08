using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace SuperAbp.Exam.Blazor;

[Dependency(ReplaceServices = true)]
public class ExamBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "考试系统";
}
