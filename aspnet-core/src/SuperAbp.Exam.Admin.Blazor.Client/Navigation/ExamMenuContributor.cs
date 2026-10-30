using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SuperAbp.Exam.Localization;
using SuperAbp.Exam.MultiTenancy;
using SuperAbp.Exam.Permissions;
using Volo.Abp.Account.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity.Blazor.MudBlazor;
using Volo.Abp.SettingManagement.Blazor.MudBlazor.Menus;
using Volo.Abp.TenantManagement.Blazor.MudBlazor.Navigation;
using Volo.Abp.UI.Navigation;

namespace SuperAbp.Exam.Admin.Blazor.Client.Navigation;

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

    private static async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<ExamResource>();

        //Administration
        var administration = context.Menu.GetAdministration();
        administration.Order = 6;
        context.Menu.AddItem(new ApplicationMenuItem(
            ExamMenus.Home,
            l["Menu:Home"],
            "/",
            icon: "fas fa-home",
            order: 1
        ));

        var questionManagement = new ApplicationMenuItem(
            ExamMenus.QuestionManagement,
            l["Menu:QuestionManagement"],
            "#",
            icon: "fas fa-question-circle",
            order: 2
        );
        questionManagement.AddItem(new ApplicationMenuItem(
            ExamMenus.Questions,
            l["Menu:Questions"],
            "/question-management/questions"
        ).RequirePermissions(ExamPermissions.Questions.Default));
        questionManagement.AddItem(new ApplicationMenuItem(
            ExamMenus.QuestionBanks,
            l["Menu:QuestionBanks"],
            "/question-management/question-banks"
        ).RequirePermissions(ExamPermissions.QuestionBanks.Default));
        context.Menu.AddItem(questionManagement);

        var paperManagement = new ApplicationMenuItem(
            ExamMenus.PaperManagement,
            l["Menu:PaperManagement"],
            "#",
            icon: "fas fa-file-alt",
            order: 3
        );
        paperManagement.AddItem(new ApplicationMenuItem(
            ExamMenus.Papers,
            l["Menu:Papers"],
            "/paper-management/papers"
        ).RequirePermissions(ExamPermissions.Papers.Default));
        context.Menu.AddItem(paperManagement);

        var examManagement = new ApplicationMenuItem(
            ExamMenus.ExamManagement,
            l["Menu:ExamManagement"],
            "#",
            icon: "fas fa-calendar-check",
            order: 4
        );
        examManagement.AddItem(new ApplicationMenuItem(
            ExamMenus.Exams,
            l["Menu:Exams"],
            "/exam-management/exams"
        ).RequirePermissions(ExamPermissions.Exams.Default));
        context.Menu.AddItem(examManagement);

        var system = new ApplicationMenuItem(
            ExamMenus.System,
            l["Menu:System"],
            "#",
            icon: "fas fa-toolbox",
            order: 5
        );
        system.AddItem(new ApplicationMenuItem(
            ExamMenus.SystemAnnouncements,
            l["Menu:Announcements"],
            "/sys/announcements"
        ).RequirePermissions(ExamPermissions.Announcements.Default));
        system.AddItem(new ApplicationMenuItem(
            ExamMenus.SystemAnnouncementCategories,
            l["Menu:AnnouncementCategories"],
            "/sys/announcement-categories"
        ).RequirePermissions(ExamPermissions.AnnouncementCategories.Default));
        system.AddItem(new ApplicationMenuItem(
            ExamMenus.SystemKnowledgePoints,
            l["Menu:KnowledgePoints"],
            "/sys/knowledge-points"
        ).RequirePermissions(ExamPermissions.KnowledgePoints.Default));
        system.AddItem(new ApplicationMenuItem(
            ExamMenus.SystemNotifications,
            l["Menu:Notifications"],
            "/sys/notifications"
        ).RequirePermissions(ExamPermissions.Notifications.Default));
        context.Menu.AddItem(system);

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
    }

    private async Task ConfigureUserMenuAsync(MenuConfigurationContext context)
    {
        if (OperatingSystem.IsBrowser())
        {
            //Blazor wasm menu items
            var authServerUrl = _configuration["AuthServer:Authority"] ?? "";
            var accountResource = context.GetLocalizer<AccountResource>();
            context.Menu.AddItem(new ApplicationMenuItem("Account.Manage", accountResource["MyAccount"], $"{authServerUrl.EnsureEndsWith('/')}Account/Manage", icon: "fa fa-cog", order: 900, target: "_blank").RequireAuthenticated());
        }
        else
        {
            //Blazor server menu items
        }
        await Task.CompletedTask;
    }
}
