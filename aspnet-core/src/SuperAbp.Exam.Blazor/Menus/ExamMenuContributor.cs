using System;
using System.Threading.Tasks;
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
                icon: "fas fa-home"
            )
        );
        if (await context.IsGrantedAsync(ExamPermissions.Exams.Default))
        {
            context.Menu.Items.Add(new ApplicationMenuItem(
                ExamMenus.Exam,
                l["Menu:MyExam"],
                "/Exam",
                icon: "fas fa-home"
            ));
            context.Menu.Items.Add(new ApplicationMenuItem(
                ExamMenus.QuestionRepository,
                l["Menu:QuestionRepository"],
                "/Repository",
                icon: "fas fa-home"
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
            icon: "fa fa-cog",
            order: 1000,
            null).RequireAuthenticated());

        return Task.CompletedTask;
    }
}
