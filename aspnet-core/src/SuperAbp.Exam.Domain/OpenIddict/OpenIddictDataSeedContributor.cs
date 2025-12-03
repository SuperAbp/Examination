using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using OpenIddict.Abstractions;
using Volo.Abp;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;

namespace SuperAbp.Exam.OpenIddict;

/* Creates initial data that is needed to property run the application
 * and make client-to-server communication possible.
 */

public class OpenIddictDataSeedContributor : OpenIddictDataSeedContributorBase, IDataSeedContributor, ITransientDependency
{
    private readonly IPermissionDataSeeder _permissionDataSeeder;
    private readonly IStringLocalizer<OpenIddictResponse> L;

    public OpenIddictDataSeedContributor(
        IConfiguration configuration,
        IOpenIddictApplicationRepository openIddictApplicationRepository,
        IAbpApplicationManager applicationManager,
        IOpenIddictScopeRepository openIddictScopeRepository,
        IOpenIddictScopeManager scopeManager,
        IPermissionDataSeeder permissionDataSeeder,
        IStringLocalizer<OpenIddictResponse> l)
        : base(configuration, openIddictApplicationRepository, applicationManager, openIddictScopeRepository, scopeManager)
    {
        _permissionDataSeeder = permissionDataSeeder;
        L = l;
    }

    [UnitOfWork]
    public virtual async Task SeedAsync(DataSeedContext context)
    {
        await CreateScopesAsync();
        await CreateApplicationsAsync();
    }

    private async Task CreateScopesAsync()
    {
        await CreateScopesAsync(new OpenIddictScopeDescriptor
        {
            Name = "Exam",
            DisplayName = "Exam API",
            Resources = { "Exam" }
        });
    }

    private async Task CreateApplicationsAsync()
    {
        var commonScopes = new List<string> {
            OpenIddictConstants.Permissions.Scopes.Address,
            OpenIddictConstants.Permissions.Scopes.Email,
            OpenIddictConstants.Permissions.Scopes.Phone,
            OpenIddictConstants.Permissions.Scopes.Profile,
            OpenIddictConstants.Permissions.Scopes.Roles,
            "Exam"
        };

        var configurationSection = Configuration.GetSection("OpenIddict:Applications");

        //Console Test / Angular Client
        var consoleAndAngularClientId = configurationSection["Exam_App:ClientId"];
        if (!consoleAndAngularClientId.IsNullOrWhiteSpace())
        {
            var consoleAndAngularClientRootUrl = configurationSection["Exam_App:RootUrl"]?.TrimEnd('/');
            await CreateOrUpdateApplicationAsync(
                applicationType: OpenIddictConstants.ApplicationTypes.Web,
                name: consoleAndAngularClientId!,
                type: OpenIddictConstants.ClientTypes.Public,
                consentType: OpenIddictConstants.ConsentTypes.Implicit,
                displayName: "Console Test / Angular Application",
                secret: null,
                grantTypes: new List<string> {
                    OpenIddictConstants.GrantTypes.AuthorizationCode,
                    OpenIddictConstants.GrantTypes.Password,
                    OpenIddictConstants.GrantTypes.ClientCredentials,
                    OpenIddictConstants.GrantTypes.RefreshToken,
                    "LinkLogin",
                    "Impersonation"
                },
                scopes: commonScopes,
                redirectUris: [consoleAndAngularClientRootUrl],
                clientUri: consoleAndAngularClientRootUrl,
                postLogoutRedirectUris: [consoleAndAngularClientRootUrl],
                logoUri: "/images/clients/angular.svg"
            );
        }

        // Blazor Client
        var blazorClientId = configurationSection["Exam_Blazor:ClientId"];
        if (!blazorClientId.IsNullOrWhiteSpace())
        {
            var blazorRootUrl = configurationSection["Exam_Blazor:RootUrl"]?.TrimEnd('/');

            await CreateOrUpdateApplicationAsync(
                applicationType: OpenIddictConstants.ApplicationTypes.Web,
                name: blazorClientId!,
                type: OpenIddictConstants.ClientTypes.Public,
                consentType: OpenIddictConstants.ConsentTypes.Implicit,
                displayName: "Blazor Application",
                secret: null,
                grantTypes: new List<string> { OpenIddictConstants.GrantTypes.AuthorizationCode, },
                scopes: commonScopes,
                redirectUris: [$"{blazorRootUrl}/authentication/login-callback"],
                clientUri: blazorRootUrl,
                postLogoutRedirectUris: [$"{blazorRootUrl}/authentication/logout-callback"],
                logoUri: "/images/clients/blazor.svg"
            );
        }

        // Blazor Server Tiered Client
        var blazorServerTieredClientId = configurationSection["Exam_BlazorServerTiered:ClientId"];
        if (!blazorServerTieredClientId.IsNullOrWhiteSpace())
        {
            var blazorServerTieredRootUrl = configurationSection["Exam_BlazorServerTiered:RootUrl"]!.EnsureEndsWith('/');

            await CreateOrUpdateApplicationAsync(
                applicationType: OpenIddictConstants.ApplicationTypes.Web,
                name: blazorServerTieredClientId!,
                type: OpenIddictConstants.ClientTypes.Confidential,
                consentType: OpenIddictConstants.ConsentTypes.Implicit,
                displayName: "Blazor Server Application",
                secret: configurationSection["Exam_BlazorServerTiered:ClientSecret"] ?? "1q2w3e*",
                grantTypes: new List<string> //Hybrid flow
                {
                    OpenIddictConstants.GrantTypes.AuthorizationCode, OpenIddictConstants.GrantTypes.Implicit
                },
                scopes: commonScopes,
                redirectUris: [$"{blazorServerTieredRootUrl}signin-oidc"],
                clientUri: blazorServerTieredRootUrl,
                postLogoutRedirectUris: [$"{blazorServerTieredRootUrl}signout-callback-oidc"],
                logoUri: "/images/clients/blazor.svg"
            );
        }

        // Swagger Client
        var swaggerClientId = configurationSection["Exam_Swagger:ClientId"];
        if (!swaggerClientId.IsNullOrWhiteSpace())
        {
            var swaggerRootUrl = configurationSection["Exam_Swagger:RootUrl"]?.TrimEnd('/');

            await CreateOrUpdateApplicationAsync(
                applicationType: OpenIddictConstants.ApplicationTypes.Web,
                name: swaggerClientId!,
                type: OpenIddictConstants.ClientTypes.Public,
                consentType: OpenIddictConstants.ConsentTypes.Implicit,
                displayName: "Swagger Application",
                secret: null,
                grantTypes: new List<string> { OpenIddictConstants.GrantTypes.AuthorizationCode, },
                scopes: commonScopes,
                redirectUris: [$"{swaggerRootUrl}/swagger/oauth2-redirect.html"],
                clientUri: swaggerRootUrl,
                logoUri: "/images/clients/swagger.svg"
            );
        }

        // Admin Swagger Client
        var swaggerAdminClientId = configurationSection["Exam_Admin_Swagger:ClientId"];
        if (!swaggerAdminClientId.IsNullOrWhiteSpace())
        {
            var swaggerAdminRootUrl = configurationSection["Exam_Admin_Swagger:RootUrl"]?.TrimEnd('/');

            await CreateOrUpdateApplicationAsync(
                applicationType: OpenIddictConstants.ApplicationTypes.Web,
                name: swaggerAdminClientId,
                type: OpenIddictConstants.ClientTypes.Public,
                consentType: OpenIddictConstants.ConsentTypes.Implicit,
                displayName: "Swagger Admin Application",
                secret: null,
                grantTypes: new List<string>
                {
                    OpenIddictConstants.GrantTypes.AuthorizationCode,
                },
                scopes: commonScopes,
                redirectUris: [$"{swaggerAdminRootUrl}/swagger/oauth2-redirect.html"],
                clientUri: swaggerAdminRootUrl,
                logoUri: "/images/clients/swagger.svg"
            );
        }

        // Addmin Console Test / Angular Client
        var consoleAndAngularAdminClientId = configurationSection["Exam_Admin_App:ClientId"];
        if (!consoleAndAngularAdminClientId.IsNullOrWhiteSpace())
        {
            var consoleAndAngularAdminClientRootUrl = configurationSection["Exam_Admin_App:RootUrl"]?.TrimEnd('/');
            await CreateOrUpdateApplicationAsync(
                applicationType: OpenIddictConstants.ApplicationTypes.Web,
                name: consoleAndAngularAdminClientId,
                type: OpenIddictConstants.ClientTypes.Public,
                consentType: OpenIddictConstants.ConsentTypes.Implicit,
                displayName: "Console Admin Test / Angular Admin Application",
                secret: null,
                grantTypes: new List<string>
                {
                    OpenIddictConstants.GrantTypes.AuthorizationCode,
                    OpenIddictConstants.GrantTypes.Password,
                    OpenIddictConstants.GrantTypes.ClientCredentials,
                    OpenIddictConstants.GrantTypes.RefreshToken
                },
                scopes: commonScopes,
                redirectUris: [consoleAndAngularAdminClientRootUrl],
                clientUri: consoleAndAngularAdminClientRootUrl,
                postLogoutRedirectUris: [consoleAndAngularAdminClientRootUrl],
                logoUri: "/images/clients/angular.svg"
            );
        }
    }
}