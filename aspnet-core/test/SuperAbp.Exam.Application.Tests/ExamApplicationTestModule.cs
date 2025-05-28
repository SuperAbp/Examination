using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SuperAbp.Exam.Admin;
using Volo.Abp.Modularity;
using Volo.Abp.Timing;

namespace SuperAbp.Exam;

[DependsOn(
    typeof(ExamApplicationAdminModule),
    typeof(ExamApplicationModule),
    typeof(ExamDomainTestModule)
)]
public class ExamApplicationTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        base.ConfigureServices(context);
        // context.Services.Replace(
        //     ServiceDescriptor.Transient<IClock, FakeClock>());
    }
}