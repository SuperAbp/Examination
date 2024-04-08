using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.Web.Theming;
using Volo.Abp.AspNetCore.Components.Web.Theming.Theming;
using Volo.Abp.Modularity;
using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;

namespace Volo.Abp.AspNetCore.Components.Web.BasicTheme;

[DependsOn(
    typeof(AbpAspNetCoreComponentsWebThemingModule)
)]
[DependsOn(typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule))]
    public class AbpAspNetCoreComponentsWebBasicThemeModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        base.ConfigureServices(context);
        context.Services.AddAntDesign();
    }
}