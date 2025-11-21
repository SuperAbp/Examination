using AntDesign;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SuperAbp.Exam.Admin.QuestionManagement.QuestionBanks;
using System;
using System.ComponentModel.DataAnnotations;
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
    protected IValidator<QuestionBankUpdateDto> Validator { get; set; }

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

    protected FormValidationRule[] GetRulesAsync(string fieldName)
    {
        var descriptor = Validator.CreateDescriptor();
        var validators = descriptor.GetValidatorsForMember(fieldName);
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