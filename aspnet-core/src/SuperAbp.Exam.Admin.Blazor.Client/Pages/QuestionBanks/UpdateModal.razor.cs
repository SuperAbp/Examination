using AntDesign;
using Microsoft.AspNetCore.Components;
using SuperAbp.Exam.Admin.QuestionManagement.QuestionBanks;
using System;
using System.Threading.Tasks;
using Volo.Abp.ObjectMapping;

namespace SuperAbp.Exam.Admin.Blazor.Client.Pages.QuestionBanks;

public partial class UpdateModal
{
    protected Guid Id { get; set; }

    private bool _submitLoading = false;
    private bool _visible = false;

    protected QuestionBankUpdateDto QuestionBank { get; set; }

    protected IForm UpdateForm { get; set; }

    [Parameter]
    public Func<Task> OnSaveSuccess { get; set; }

    [Inject]
    protected IObjectMapper ObjectMapper { get; set; }

    [Inject]
    protected IQuestionBankAdminAppService QuestionBankAppService { get; set; }

    public virtual async Task OpenAsync(Guid id)
    {
        Id = id;
        QuestionBankDetailDto dto = await QuestionBankAppService.GetAsync(id);
        QuestionBank = new QuestionBankUpdateDto() { Title = dto.Title, Remark = dto.Remark };

        _visible = true;
        await InvokeAsync(StateHasChanged);
    }

    protected async Task SaveAsync()
    {
        try
        {
            var validate = true;
            if (UpdateForm != null)
            {
                validate = UpdateForm.Validate();
            }
            if (!validate)
            {
                return;
            }

            _submitLoading = true;
            StateHasChanged();
            await QuestionBankAppService.UpdateAsync(Id, QuestionBank);
        }
        finally
        {
            _submitLoading = false;
        }
        Close();
        if (OnSaveSuccess is not null)
        {
            await OnSaveSuccess();
        }
    }

    protected void Close()
    {
        _visible = false;
    }
}