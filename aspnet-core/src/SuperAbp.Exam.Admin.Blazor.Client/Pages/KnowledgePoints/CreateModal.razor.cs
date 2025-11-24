using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SuperAbp.Exam.Admin.KnowledgePoints;
using System;
using System.Threading.Tasks;

namespace SuperAbp.Exam.Admin.Blazor.Client.Pages.KnowledgePoints;

public partial class CreateModal
{
    private bool _submitLoading = false;
    private bool _visible = false;
    protected Guid? ParentId { get; set; }
    protected IForm CreateForm { get; set; }
    protected KnowledgePointCreateDto knowledgePoint { get; set; }

    [Parameter]
    public Func<Task> OnSaveSuccess { get; set; }

    [Inject]
    protected IKnowledgePointAdminAppService KnowledgePointAppService { get; set; }

    public virtual async Task OpenAsync(Guid? parentId = null)
    {
        ParentId = parentId;
        knowledgePoint = new KnowledgePointCreateDto() { ParentId = parentId, Name = String.Empty };

        _visible = true;
        await InvokeAsync(StateHasChanged);
    }

    protected async Task SaveAsync(MouseEventArgs e)
    {
        try
        {
            var validate = true;
            if (CreateForm != null)
            {
                validate = CreateForm.Validate();
            }
            if (!validate)
            {
                return;
            }

            _submitLoading = true;
            StateHasChanged();
            await KnowledgePointAppService.CreateAsync(knowledgePoint);
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