using Microsoft.AspNetCore.Components;

namespace SuperAbp.Exam.Admin.Blazor.Client.Pages;

public partial class Index
{
    [Inject]
    protected NavigationManager Navigation { get; set; } = default!;

    private void Login()
    {
        Navigation.NavigateTo("/Account/Login", true);
    }

    protected override void Dispose(bool disposing)
    {
        PageLayout.ShowToolbar = true;
    }
}
