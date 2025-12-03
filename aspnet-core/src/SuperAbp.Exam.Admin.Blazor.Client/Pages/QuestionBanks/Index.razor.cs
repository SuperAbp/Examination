using AntDesign;
using Lsw.Abp.AntDesignUI;
using Lsw.Abp.AspnetCore.Components.Web.AntDesignTheme.PageToolbars;
using Lsw.Abp.IdentityManagement.Blazor.AntDesignUI;
using Microsoft.AspNetCore.Authorization;
using SuperAbp.Exam.Admin.QuestionManagement.QuestionBanks;
using SuperAbp.Exam.Localization;
using SuperAbp.Exam.Permissions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Localization;
using Volo.Abp.ObjectExtending;

namespace SuperAbp.Exam.Admin.Blazor.Client.Pages.QuestionBanks;

[Authorize(ExamPermissions.QuestionBanks.Management)]
public partial class Index
{
    public Index()
    {
        ObjectMapperContext = typeof(ExamBlazorClientModule);
        LocalizationResource = typeof(ExamResource);

        CreatePolicyName = ExamPermissions.QuestionBanks.Create;
        UpdatePolicyName = ExamPermissions.QuestionBanks.Update;
        DeletePolicyName = ExamPermissions.QuestionBanks.Delete;
    }

    protected List<TableColumn> QuestionBankTableColumns => TableColumns.Get<Index>();

    protected IForm SearchForm { get; set; }

    protected PageToolbar Toolbar { get; } = new();
    protected CreateModal CreateModal { get; set; }
    protected UpdateModal UpdateModal { get; set; }

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
                            await OpenEditModalAsync(data.As<QuestionBankListDto>().Id);
                        }
                    },
                    new EntityAction
                    {
                        Text = L["Delete"],
                        Visible = (data) => HasDeletePermission,
                        Clicked = async (data) => await DeleteAsync(data.As<QuestionBankListDto>().Id),
                        ConfirmationMessage = (data) => UiLocalizer["ItemWillBeDeletedMessage"]
                    }
            ]);

        return base.SetEntityActionsAsync();
    }

    protected override ValueTask SetTableColumnsAsync()
    {
        QuestionBankTableColumns
            .AddRange(new TableColumn[]
            {
                new TableColumn
                    {
                        Title = L["Title"],
                        Data = nameof(QuestionBankListDto.Title)
                    },
               new TableColumn
               {
                        Title = L["SingleCount"],
                        Data = nameof(QuestionBankListDto.SingleCount)
               },
               new TableColumn{
                        Title = L["MultiCount"],
                        Data = nameof(QuestionBankListDto.MultiCount)
                    },
               new TableColumn{
                        Title = L["JudgeCount"],
                        Data = nameof(QuestionBankListDto.JudgeCount)
                    },
             new  TableColumn {
                        Title = L["BlankCount"],
                        Data = nameof(QuestionBankListDto.BlankCount)
                    }
            });

        QuestionBankTableColumns.AddRange(GetExtensionTableColumns(IdentityModuleExtensionConsts.ModuleName,
            IdentityModuleExtensionConsts.EntityNames.Role));

        QuestionBankTableColumns.Add(new TableColumn
        {
            Title = L["Actions"],
            Actions = EntityActions.Get<Index>()
        });

        return base.SetTableColumnsAsync();
    }

    protected override async ValueTask SetToolbarItemsAsync()
    {
        Toolbar.AddButton(L["NewQuestionBank"], OpenCreateModalAsync,
            IconType.Outline.Plus,
            requiredPolicyName: CreatePolicyName);
        await base.SetToolbarItemsAsync();
    }

    protected override async ValueTask SetBreadcrumbItemsAsync()
    {
        BreadcrumbItems.Add(new AbpBreadcrumbItem(L["Menu:ExamManagement"]));
        BreadcrumbItems.Add(new AbpBreadcrumbItem(L["QuestionBanks"], "question-banks"));
        await base.SetBreadcrumbItemsAsync();
    }

    protected virtual async Task ClearSearchAsync()
    {
        SearchForm.Reset();
        await SearchEntitiesAsync();
    }

    protected override async Task<PagedResultDto<QuestionBankListDto>> GetListAsync(GetQuestionBanksInput input)
    {
        return await AppService.GetListAsync(input);
    }

    protected virtual async Task OpenCreateModalAsync()
    {
        await CreateModal.OpenAsync();
    }

    protected virtual async Task OpenEditModalAsync(Guid id)
    {
        await UpdateModal.OpenAsync(id);
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