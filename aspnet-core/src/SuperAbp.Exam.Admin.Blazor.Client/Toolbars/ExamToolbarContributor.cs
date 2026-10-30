using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;

namespace SuperAbp.Exam.Admin.Blazor.Client.Toolbars;

public class ExamToolbarContributor : IToolbarContributor
{
    public Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
    {
        if (context.Toolbar.Name == StandardToolbars.Main)
        {
            context.Toolbar.Items.Add(new ToolbarItem(typeof(Shared.NotificationBell)));
        }

        return Task.CompletedTask;
    }
}
