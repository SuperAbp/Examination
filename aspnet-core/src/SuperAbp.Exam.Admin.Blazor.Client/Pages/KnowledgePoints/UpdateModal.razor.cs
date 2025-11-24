using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SuperAbp.Exam.Admin.KnowledgePoints;
using SuperAbp.Exam.Admin.QuestionManagement.QuestionBanks;
using System;
using System.Threading.Tasks;

namespace SuperAbp.Exam.Admin.Blazor.Client.Pages.KnowledgePoints;

public partial class UpdateModal
{
    private bool _submitLoading = false;
    private bool _visible = false;
    protected Guid Id { get; set; }

    protected IForm UpdateForm { get; set; }
    protected KnowledgePointUpdateDto knowledgePoint { get; set; }

    [Parameter]
    public Func<Task> OnSaveSuccess { get; set; }

    [Inject]
    protected IKnowledgePointAdminAppService KnowledgePointAppService { get; set; }

    public virtual async Task OpenAsync(Guid id)
    {
        Id = id;
        GetKnowledgePointForEditorOutput dto = await KnowledgePointAppService.GetEditorAsync(id);
        knowledgePoint = new KnowledgePointUpdateDto() { ParentId = dto.ParentId, Name = dto.Name };

        _visible = true;
        await InvokeAsync(StateHasChanged);
    }

    protected async Task SaveAsync(MouseEventArgs e)
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
            await KnowledgePointAppService.UpdateAsync(Id, knowledgePoint);
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