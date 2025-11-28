using Microsoft.AspNetCore.Components;

namespace SuperAbp.Exam.Admin.Blazor.Client.Components;

public partial class YNComponent
{
    [Parameter]
    public bool Value { get; set; }
}