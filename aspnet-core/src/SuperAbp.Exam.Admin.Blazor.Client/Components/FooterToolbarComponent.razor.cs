using Microsoft.AspNetCore.Components;

namespace SuperAbp.Exam.Admin.Blazor.Client.Components;

public partial class FooterToolbarComponent
{
    [Parameter]
    public RenderFragment? Content { get; set; }

    [Parameter]
    public RenderFragment? Extra { get; set; }
}