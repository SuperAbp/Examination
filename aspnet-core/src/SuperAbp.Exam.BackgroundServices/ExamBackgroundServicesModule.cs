using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuperAbp.Exam.BackgroundServices.Exams;
using SuperAbp.Exam.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Modularity;

namespace SuperAbp.Exam.BackgroundServices;

[DependsOn(
    typeof(ExamEntityFrameworkCoreModule),
    typeof(AbpAutofacModule),
    typeof(AbpBackgroundWorkersModule)
)]
public class ExamBackgroundServicesModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var hostEnvironment = context.Services.GetSingletonInstance<IHostEnvironment>();

        context.Services.AddHostedService<ExamBackgroundServicesHostedService>();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        context.AddBackgroundWorkerAsync<SubmittedUserExamWorker>();
        context.AddBackgroundWorkerAsync<TerminateExamWorker>();
        context.AddBackgroundWorkerAsync<InitialDataWorker>();
    }
}