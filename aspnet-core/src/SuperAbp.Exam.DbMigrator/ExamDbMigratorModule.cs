using SuperAbp.Exam.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace SuperAbp.Exam.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(ExamEntityFrameworkCoreModule),
    typeof(ExamApplicationContractsModule)
    )]
public class ExamDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
    }
}
