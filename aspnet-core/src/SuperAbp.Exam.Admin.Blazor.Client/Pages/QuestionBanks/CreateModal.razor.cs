using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SuperAbp.Exam.Admin.QuestionManagement.QuestionBanks;
using System;
using System.Threading.Tasks;
using Volo.Abp.ObjectMapping;

namespace SuperAbp.Exam.Admin.Blazor.Client.Pages.QuestionBanks;

public partial class CreateModal
{
    private bool _submitLoading = false;
    private bool _visible = false;
    protected QuestionBankCreateDto QuestionBank { get; set; }

    [Parameter]
    public Func<Task> OnSaveSuccess { get; set; }

    [Inject]
    protected IQuestionBankAdminAppService QuestionBankAppService { get; set; }

    public virtual async Task OpenAsync()
    {
        QuestionBank = new QuestionBankCreateDto() { Title = String.Empty };

        _visible = true;
        await InvokeAsync(StateHasChanged);
    }

    protected async Task SaveAsync(MouseEventArgs e)
    {
        try
        {
            _submitLoading = true;
            await QuestionBankAppService.CreateAsync(QuestionBank);
        }
        finally
        {
            _submitLoading = false;
        }
        if (OnSaveSuccess is not null)
        {
            await OnSaveSuccess();
        }
    }
}