using SuperAbp.AuditLogging;
using SuperAbp.MenuManagement;
using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;

namespace SuperAbp.Exam.Admin;

[DependsOn(
    typeof(ExamApplicationContractsSharedModule),
    typeof(AbpFluentValidationModule),
    typeof(SuperAbpMenuManagementApplicationContractsModule),
    typeof(SuperAbpAuditLoggingApplicationContractsModule))]
public class ExamApplicationAdminContractsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        ExamDtoExtensions.Configure();
    }
}