using Volo.Abp.Bundling;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.Theming;

public class ComponentsComponentsBundleContributor : IBundleContributor
{
    public void AddScripts(BundleContext context)
    {
        context.Add("_content/Microsoft.AspNetCore.Components.WebAssembly.Authentication/AuthenticationService.js");
        context.Add("_content/Volo.Abp.AspNetCore.Components.Web/libs/abp/js/abp.js");
        context.Add("_content/Volo.Abp.AspNetCore.Components.Web/libs/abp/js/lang-utils.js");
        context.Add("_content/Volo.Abp.AspNetCore.Components.Web/libs/abp/js/authentication-state-listener.js");
    }

    public void AddStyles(BundleContext context)
    {
    }
}