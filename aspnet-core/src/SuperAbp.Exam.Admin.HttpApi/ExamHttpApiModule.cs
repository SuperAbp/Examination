using Localization.Resources.AbpUi;
using SuperAbp.AuditLogging;
using SuperAbp.Exam.Localization;
using SuperAbp.MenuManagement;
using Volo.Abp.Account;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace SuperAbp.Exam.Admin;

[DependsOn(
    typeof(ExamApplicationContractsModule),
    typeof(AbpAccountHttpApiModule),
    typeof(AbpIdentityHttpApiModule),
    typeof(AbpPermissionManagementHttpApiModule),
    typeof(AbpTenantManagementHttpApiModule),
    typeof(AbpFeatureManagementHttpApiModule),
    typeof(AbpSettingManagementHttpApiModule),
    typeof(SuperAbpMenuManagementHttpApiModule),
    typeof(SuperAbpAuditLoggingHttpApiModule)
    )]
public class ExamHttpApiModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureLocalization();
    }

    private void ConfigureLocalization()
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<ExamResource>()
                .AddBaseTypes(
                    typeof(AbpUiResource)
                );
        });
    }
}