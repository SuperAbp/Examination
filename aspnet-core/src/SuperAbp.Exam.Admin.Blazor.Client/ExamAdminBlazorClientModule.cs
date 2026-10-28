using System;
using System.Net.Http;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SuperAbp.Exam.Admin.Blazor.Client.Navigation;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.AspNetCore.Components.Web.Theming.MudBlazor.Routing;
using Volo.Abp.AspNetCore.Components.WebAssembly.MudBlazorLeptonXLiteTheme;
using Volo.Abp.Autofac.WebAssembly;
using Volo.Abp.FeatureManagement.Blazor.MudBlazor.WebAssembly;
using Volo.Abp.Identity.Blazor.MudBlazor.WebAssembly;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement.Blazor.MudBlazor.WebAssembly;
using Volo.Abp.TenantManagement.Blazor.MudBlazor.WebAssembly;
using Volo.Abp.UI.Navigation;

namespace SuperAbp.Exam.Admin.Blazor.Client;

[DependsOn(
    typeof(AbpSettingManagementBlazorMudBlazorWebAssemblyModule),
    typeof(AbpFeatureManagementBlazorMudBlazorWebAssemblyModule),
    typeof(AbpIdentityBlazorMudBlazorWebAssemblyModule),
    typeof(AbpTenantManagementBlazorMudBlazorWebAssemblyModule),
    typeof(AbpAspNetCoreComponentsWebAssemblyMudBlazorLeptonXLiteThemeModule),
    typeof(AbpAutofacWebAssemblyModule),
    typeof(ExamHttpApiClientModule)
)]
public class ExamAdminBlazorClientModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpAspNetCoreComponentsWebOptions>(options =>
        {
            options.IsBlazorWebApp = true;
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var environment = context.Services.GetSingletonInstance<IWebAssemblyHostEnvironment>();
        var builder = context.Services.GetSingletonInstance<WebAssemblyHostBuilder>();

        ConfigureAuthentication(builder);
        ConfigureHttpClient(context, environment);
        ConfigureBlazorise(context);
        ConfigureRouter(context);
        ConfigureMenu(context);
    }

    private void ConfigureRouter(ServiceConfigurationContext context)
    {
        Configure<AbpRouterOptions>(options =>
        {
            options.AppAssembly = typeof(ExamAdminBlazorClientModule).Assembly;
            options.AdditionalAssemblies.Add(typeof(ExamAdminBlazorClientModule).Assembly);
        });
    }

    private void ConfigureMenu(ServiceConfigurationContext context)
    {
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new ExamMenuContributor(context.Services.GetConfiguration()));
        });
    }

    private void ConfigureBlazorise(ServiceConfigurationContext context)
    {
        // MudBlazor services are registered by AbpMudBlazorUIModule.
    }

    private static void ConfigureAuthentication(WebAssemblyHostBuilder builder)
    {
        builder.Services.AddBlazorWebAppServices();
    }

    private static void ConfigureHttpClient(ServiceConfigurationContext context, IWebAssemblyHostEnvironment environment)
    {
        context.Services.AddTransient(sp => new HttpClient
        {
            BaseAddress = new Uri(environment.BaseAddress)
        });
    }
}