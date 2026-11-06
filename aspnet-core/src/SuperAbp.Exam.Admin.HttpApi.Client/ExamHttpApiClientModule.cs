using Microsoft.Extensions.DependencyInjection;
using SuperAbp.Exam.Admin;
using SuperAbp.Exam.Options;
using Volo.Abp.Account;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using Volo.Abp.VirtualFileSystem;

namespace SuperAbp.Exam.Admin;

[DependsOn(
    typeof(ExamAdminApplicationContractsModule),
    typeof(AbpPermissionManagementHttpApiClientModule),
    typeof(AbpFeatureManagementHttpApiClientModule),
    typeof(AbpAccountHttpApiClientModule),
    typeof(AbpIdentityHttpApiClientModule),
    typeof(AbpTenantManagementHttpApiClientModule),
    typeof(AbpSettingManagementHttpApiClientModule)
)]
public class ExamHttpApiClientModule : AbpModule
{
    public const string RemoteServiceName = "Default";

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(ExamAdminApplicationContractsModule).Assembly,
            RemoteServiceName
        );

        // Application.Contracts.Shared 里的选项接口（IOptionAppService）也需要注册动态代理，
        // 否则 WASM 端注入直接失败（页面渲染崩溃）。
        context.Services.AddHttpClientProxies(
            typeof(IOptionAppService).Assembly,
            RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<ExamHttpApiClientModule>();
        });
    }
}
