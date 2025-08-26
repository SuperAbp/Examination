using System;
using System.Threading.Tasks;
using AntDesign;
using Microsoft.Extensions.Configuration;
using SuperAbp.Exam.Localization;
using SuperAbp.Exam.Permissions;
using Volo.Abp.Account.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.UI.Navigation;

namespace SuperAbp.Exam.Blazor.Menus;

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

    private async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
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
        if (await context.IsGrantedAsync(ExamPermissions.Exams.Default))
        {
            context.Menu.Items.Add(new ApplicationMenuItem(
                ExamMenus.Exam,
                l["Menu:OnlineExam"],
                "/exam",
                icon: IconType.Outline.Home
            ));
            var identityMenuItem = new ApplicationMenuItem(ExamMenus.My, l["Menu:My"], icon: IconType.Outline.User);
            context.Menu.Items.Add(identityMenuItem);
            identityMenuItem.Items.Add(new ApplicationMenuItem(
                ExamMenus.MyExam,
                l["Menu:MyExam"],
                "/my/exam",
                icon: IconType.Outline.Home
            ));
            identityMenuItem.Items.Add(new ApplicationMenuItem(
                ExamMenus.MyFavorite,
                l["Menu:MyFavorite"],
                "/my/favorite",
                icon: IconType.Outline.Star
            ));
            identityMenuItem.Items.Add(new ApplicationMenuItem(
                ExamMenus.MyMistakeReview,
                l["Menu:MyMistakeReview"],
                "/my/mistakes-review",
                icon: IconType.Outline.Star
            ));
            context.Menu.Items.Add(new ApplicationMenuItem(
                ExamMenus.QuestionBank,
                l["Menu:QuestionBank"],
                "/question-bank",
                icon: IconType.Outline.History
            ));
        }
    }

    private Task ConfigureUserMenuAsync(MenuConfigurationContext context)
    {
        var accountStringLocalizer = context.GetLocalizer<AccountResource>();

        var authServerUrl = _configuration["AuthServer:Authority"] ?? "";

        context.Menu.AddItem(new ApplicationMenuItem(
            "Account.Manage",
            accountStringLocalizer["MyAccount"],
            $"{authServerUrl.EnsureEndsWith('/')}Account/Manage?returnUrl={_configuration["App:SelfUrl"]}",
            icon: IconType.Outline.Setting,
            order: 1000,
            null).RequireAuthenticated());

        return Task.CompletedTask;
    }
}