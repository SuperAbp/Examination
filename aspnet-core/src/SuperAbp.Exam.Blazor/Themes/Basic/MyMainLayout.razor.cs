using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;
using Volo.Abp.UI.Navigation;

namespace SuperAbp.Exam.Blazor.Themes.Basic
{
    public partial class MyMainLayout
    {
        [Inject]
        protected IMenuManager MenuManager { get; set; }
        [Inject]
        private IToolbarManager ToolbarManager { get; set; }
        protected ApplicationMenu Menu { get; set; }
        protected List<RenderFragment> ToolbarItemRenders { get; set; } = new List<RenderFragment>();

        protected override async Task OnInitializedAsync()
        {
            Menu = await MenuManager.GetMainMenuAsync().ConfigureAwait(continueOnCapturedContext: false);
            await GetToolbarItemRendersAsync();
        }
        private async Task GetToolbarItemRendersAsync()
        {
            var toolbar = await ToolbarManager.GetAsync(StandardToolbars.Main);

            ToolbarItemRenders.Clear();

            var sequence = 0;
            foreach (var item in toolbar.Items)
            {
                ToolbarItemRenders.Add(builder =>
                {
                    builder.OpenComponent(sequence++, item.ComponentType);
                    builder.CloseComponent();
                });
            }
        }
    }
}
