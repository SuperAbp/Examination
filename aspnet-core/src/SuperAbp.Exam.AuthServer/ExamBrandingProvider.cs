using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace SuperAbp.Exam;

[Dependency(ReplaceServices = true)]
public class ExamBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "Exam";
}
