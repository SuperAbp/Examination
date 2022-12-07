using Volo.Abp.Modularity;

namespace SuperAbp.Exam.Admin;

[DependsOn(
    typeof(ExamApplicationContractsSharedModule)
)]
public class ExamApplicationContractsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        ExamDtoExtensions.Configure();
    }
}