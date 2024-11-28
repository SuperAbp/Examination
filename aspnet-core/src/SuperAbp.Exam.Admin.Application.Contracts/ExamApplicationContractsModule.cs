using SuperAbp.AuditLogging;
using SuperAbp.MenuManagement;
using Volo.Abp.Account;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace SuperAbp.Exam.Admin;

[DependsOn(
    typeof(ExamApplicationContractsSharedModule),
    typeof(SuperAbpMenuManagementApplicationContractsModule),
    typeof(SuperAbpAuditLoggingApplicationContractsModule))]
public class ExamApplicationContractsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        ExamDtoExtensions.Configure();
    }
}