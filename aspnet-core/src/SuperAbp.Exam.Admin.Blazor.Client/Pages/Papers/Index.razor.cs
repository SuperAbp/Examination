using AntDesign;
using Lsw.Abp.AntDesignUI;
using Lsw.Abp.AspnetCore.Components.Web.AntDesignTheme.PageToolbars;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using SuperAbp.Exam.Admin.Blazor.Client.Components;
using SuperAbp.Exam.Admin.PaperManagement.Papers;
using SuperAbp.Exam.Admin.QuestionManagement.Questions;
using SuperAbp.Exam.Localization;
using SuperAbp.Exam.Permissions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.ObjectExtending;

namespace SuperAbp.Exam.Admin.Blazor.Client.Pages.Papers;

[Authorize(ExamPermissions.Papers.Default)]
public partial class Index
{
    public Index()
    {
        ObjectMapperContext = typeof(ExamBlazorClientModule);
        LocalizationResource = typeof(ExamResource);

        CreatePolicyName = ExamPermissions.Papers.Create;
        UpdatePolicyName = ExamPermissions.Papers.Update;
        DeletePolicyName = ExamPermissions.Papers.Delete;
    }

    protected List<TableColumn> QuesionTableColumns => TableColumns.Get<Index>();

    protected IForm SearchForm { get; set; }

    protected PageToolbar Toolbar { get; } = new();

    [Inject]
    protected NavigationManager Navigation { get; set; }

    protected override ValueTask SetEntityActionsAsync()
    {
        EntityActions
            .Get<Index>()
            .AddRange(
            [
                    new EntityAction
                {
                    Primary = true,
                    Text = L["Edit"],
                    Visible = (data) => HasUpdatePermission,
                    Clicked = async (data) =>
                    {
                        await GoUpdateAsync(data.As<PaperListDto>().Id);
                    }
                },
                new EntityAction
                {
                    Text = L["Delete"],
                    Visible = (data) => HasDeletePermission,
                    Clicked = async (data) => await DeleteAsync(data.As<PaperListDto>().Id),
                    ConfirmationMessage = (data) => UiLocalizer["ItemWillBeDeletedMessage"]
                }
            ]);

        return base.SetEntityActionsAsync();
    }

    protected override ValueTask SetTableColumnsAsync()
    {
        QuesionTableColumns
            .AddRange(
            [
                new TableColumn
                {
                    Title = L["Name"],
                    Data = nameof(PaperListDto.Name)
                },
                new TableColumn
                {
                    Title = L["Score"],
                    Data = nameof(PaperListDto.Score),
                    Width = "60",
                },
                new TableColumn
                {
                    Title = L["ManualReview"],
                    Data = nameof(PaperListDto.ManualReview),
                    Component = typeof(ManualReviewComponent)
                }
            ]);

        QuesionTableColumns.AddRange(GetExtensionTableColumns(IdentityModuleExtensionConsts.ModuleName,
            IdentityModuleExtensionConsts.EntityNames.Role));

        QuesionTableColumns.Add(new TableColumn
        {
            Title = L["Actions"],
            Actions = EntityActions.Get<Index>()
        });

        return base.SetTableColumnsAsync();
    }

    protected override async ValueTask SetToolbarItemsAsync()
    {
        Toolbar.AddButton(L["NewQuestion"], GoCreateAsync,
            IconType.Outline.Plus,
            requiredPolicyName: ExamPermissions.QuestionBanks.Create);
        await base.SetToolbarItemsAsync();
    }

    protected override async ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new AbpBreadcrumbItem(L["Menu:ExamManagement"]));
        BreadcrumbItems.Add(new AbpBreadcrumbItem(L["QuestionBanks"], "/questions"));
        await base.SetBreadcrumbItemsAsync();
    }

    protected virtual async Task ClearSearchAsync()
    {
        SearchForm.Reset();
        await SearchEntitiesAsync();
    }

    protected override async Task<PagedResultDto<PaperListDto>> GetListAsync(GetPapersInput input)
    {
        return await AppService.GetListAsync(input);
    }

    protected virtual Task GoCreateAsync()
    {
        Navigation.NavigateTo("/papers/new", true);
        return Task.CompletedTask;
    }

    protected virtual Task GoUpdateAsync(Guid id)
    {
        // TODO: Should be remove true
        Navigation.NavigateTo("/papers/" + id, true);
        return Task.CompletedTask;
    }

    protected async Task OnSaveSuccessAsync()
    {
        // TODO: Use `ModalService` to create modal;
        await SearchEntitiesAsync();
    }

    protected async Task DeleteAsync(Guid id)
    {
        await AppService.DeleteAsync(id);
        await SearchEntitiesAsync();
    }
}