using AntDesign;
using Lsw.Abp.AntDesignUI;
using Lsw.Abp.AspnetCore.Components.Web.AntDesignTheme.PageToolbars;
using Microsoft.AspNetCore.Components;
using SuperAbp.Exam.Admin.KnowledgePoints;
using SuperAbp.Exam.Localization;
using SuperAbp.Exam.Permissions;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.Blazor.Client.Pages.KnowledgePoints;

public partial class Index
{
    protected PageToolbar Toolbar { get; } = new();

    protected CreateModal CreateModal { get; set; }
    protected UpdateModal UpdateModal { get; set; }

    [Inject]
    protected IKnowledgePointAdminAppService knowledgePointAppService { get; set; }

    public Index()
    {
        ObjectMapperContext = typeof(ExamBlazorClientModule);
        LocalizationResource = typeof(ExamResource);

        CreatePolicyName = ExamPermissions.KnowledgePoints.Create;
        UpdatePolicyName = ExamPermissions.KnowledgePoints.Update;
        DeletePolicyName = ExamPermissions.KnowledgePoints.Delete;
    }

    protected override ValueTask SetToolbarItemsAsync()
    {
        Toolbar.AddButton(L["NewKnowledgePoint"], () => { return OpenCreateModalAsync(); },
            IconType.Outline.Plus,
            requiredPolicyName: CreatePolicyName);

        return ValueTask.CompletedTask;
    }

    protected virtual async Task OpenCreateModalAsync(Guid? parentId = null)
    {
        await CreateModal.OpenAsync(parentId);
    }

    protected virtual async Task OpenUpdateModalAsync(Guid id)
    {
        await UpdateModal.OpenAsync(id);
    }

    protected override ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new AbpBreadcrumbItem(L["Menu:SystemManagement"]));
        BreadcrumbItems.Add(new AbpBreadcrumbItem(L["KnowledgePoints"]));

        return ValueTask.CompletedTask;
    }

    protected override async Task<PagedResultDto<KnowledgePointNodeDto>> GetListAsync(GetKnowledgePointsInput input)
    {
        var result = await knowledgePointAppService.GetAllAsync(input);
        return new PagedResultDto<KnowledgePointNodeDto>
        {
            Items = result.Items,
            TotalCount = result.Items.Count
        };
    }

    protected async Task OnSaveSuccessAsync()
    {
        await SearchEntitiesAsync();
    }

    protected async Task DeleteAsync(Guid id)
    {
        await AppService.DeleteAsync(id);
        await SearchEntitiesAsync();
    }
}