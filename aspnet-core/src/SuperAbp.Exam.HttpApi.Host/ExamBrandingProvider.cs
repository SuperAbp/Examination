using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace SuperAbp.Exam;

[Dependency(ReplaceServices = true)]
public class ExamBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "Exam";
}
