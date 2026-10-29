using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace SuperAbp.Exam.Blazor;

public class ExamAdminBlazorModuleAuthenticationTests
{
    [Fact]
    public async Task Configures_Cookie_And_Remote_Oidc_Authentication_Schemes()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
            [
                new KeyValuePair<string, string?>("AuthServer:Authority", "https://localhost:44386"),
                new KeyValuePair<string, string?>("AuthServer:RequireHttpsMetadata", "true"),
                new KeyValuePair<string, string?>("AuthServer:ClientId", "Exam_BlazorServerTiered"),
                new KeyValuePair<string, string?>("AuthServer:ClientSecret", "test-secret")
            ])
            .Build();

        services.AddSingleton<IConfiguration>(configuration);
        var context = new ServiceConfigurationContext(services);
        var module = new global::SuperAbp.Exam.Admin.Blazor.ExamAdminBlazorModule();
        InvokeConfigureAuthentication(module, context);

        await using var provider = services.BuildServiceProvider();
        var schemeProvider = provider.GetRequiredService<IAuthenticationSchemeProvider>();
        var schemes = await schemeProvider.GetAllSchemesAsync();

        schemes.ShouldContain(scheme => scheme.Name == "Cookies");
        schemes.ShouldContain(scheme => scheme.Name == "oidc");

        var oidcOptions = provider.GetRequiredService<Microsoft.Extensions.Options.IOptionsMonitor<OpenIdConnectOptions>>()
            .Get("oidc");
        oidcOptions.Authority.ShouldBe("https://localhost:44386");
        oidcOptions.ClientId.ShouldBe("Exam_BlazorServerTiered");
        oidcOptions.ClientSecret.ShouldBe("test-secret");
        oidcOptions.PushedAuthorizationBehavior.ShouldBe(PushedAuthorizationBehavior.Disable);
    }

    private static void InvokeConfigureAuthentication(
        global::SuperAbp.Exam.Admin.Blazor.ExamAdminBlazorModule module,
        ServiceConfigurationContext context)
    {
        var method = typeof(global::SuperAbp.Exam.Admin.Blazor.ExamAdminBlazorModule)
            .GetMethod("ConfigureAuthentication", BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("ConfigureAuthentication method was not found.");

        if (method.GetParameters().Length != 2)
        {
            throw new InvalidOperationException("ConfigureAuthentication must accept the configuration.");
        }

        method.Invoke(module, [context, context.Services.GetConfiguration()]);
    }
}
