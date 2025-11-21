using AntDesign;
using Lsw.Abp.IdentityManagement.Blazor.AntDesignUI;
using Lsw.Abp.SettingManagement.Blazor.AntDesignUI;
using Lsw.Abp.TenantManagement.Blazor.AntDesignUI;
using Microsoft.Extensions.Configuration;
using SuperAbp.Exam.Localization;
using SuperAbp.Exam.MultiTenancy;
using SuperAbp.Exam.Permissions;
using System;
using System.Threading.Tasks;
using Volo.Abp.Account.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.UI.Navigation;

namespace SuperAbp.Exam.Admin.Blazor.Client.Menus;

public class ExamMenuContributor : IMenuContributor
{
    private readonly IConfiguration _configuration;

    public ExamMenuContributor(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
        else if (context.Menu.Name == StandardMenus.User)
        {
            await ConfigureUserMenuAsync(context);
        }
    }

    private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<ExamResource>();

        context.Menu.Items.Insert(
            0,
            new ApplicationMenuItem(
                ExamMenus.Home,
                l["Menu:Home"],
                "/",
                icon: IconType.Outline.Home
            )
        );
        var examItem = new ApplicationMenuItem(IdentityMenuNames.GroupName, l["Menu:ExamManagement"],
            icon: IconType.Outline.Appstore);
        context.Menu.Items.Add(examItem);
        examItem.Items.AddRange([
            new ApplicationMenuItem(
                ExamMenus.QuestionBank,
                l["Menu:QuestionBank"],
                "/question-banks",
                requiredPermissionName: ExamPermissions.QuestionBanks.Default
            ),
            new ApplicationMenuItem(
                ExamMenus.Question,
                l["Menu:Question"],
                "/questions",
                requiredPermissionName: ExamPermissions.QuestionBanks.Default
            )
        ]);

        var administration = context.Menu.GetAdministration();

        if (MultiTenancyConsts.IsEnabled)
        {
            administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
        }
        else
        {
            administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
        }

        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
        administration.SetSubItemOrder(SettingManagementMenus.GroupName, 3);

        return Task.CompletedTask;
    }

    private Task ConfigureUserMenuAsync(MenuConfigurationContext context)
    {
        if (!OperatingSystem.IsBrowser())
        {
            return Task.CompletedTask;
        }

        var authServerUrl = _configuration["AuthServer:Authority"] ?? "";
        var accountStringLocalizer = context.GetLocalizer<AccountResource>();

        context.Menu.AddItem(new ApplicationMenuItem(
                "Account.Manage",
                accountStringLocalizer["MyAccount"],
                $"{authServerUrl.EnsureEndsWith('/')}Account/Manage",
                icon: IconType.Outline.Setting,
                order: 1000,
                target: "_blank")
            .RequireAuthenticated());

        return Task.CompletedTask;
    }
}