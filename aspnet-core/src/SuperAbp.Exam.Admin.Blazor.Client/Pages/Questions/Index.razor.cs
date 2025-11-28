using AntDesign;
using Lsw.Abp.AntDesignUI;
using Lsw.Abp.AspnetCore.Components.Web.AntDesignTheme.PageToolbars;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using SuperAbp.Exam.Admin.Options;
using SuperAbp.Exam.Admin.QuestionManagement.QuestionBanks;
using SuperAbp.Exam.Admin.QuestionManagement.Questions;
using SuperAbp.Exam.Localization;
using SuperAbp.Exam.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.ObjectExtending;

namespace SuperAbp.Exam.Admin.Blazor.Client.Pages.Questions;

[Authorize(ExamPermissions.Questions.Management)]
public partial class Index
{
    public Index()
    {
        ObjectMapperContext = typeof(ExamBlazorClientModule);
        LocalizationResource = typeof(ExamResource);

        CreatePolicyName = ExamPermissions.Questions.Create;
        UpdatePolicyName = ExamPermissions.Questions.Update;
        DeletePolicyName = ExamPermissions.Questions.Delete;
    }

    protected Dictionary<int, string> QuestionTypes { get; set; }
    protected IReadOnlyList<QuestionBankListDto> QuestionBanks { get; set; }
    protected List<TableColumn> QuesionTableColumns => TableColumns.Get<Index>();

    protected IForm SearchForm { get; set; }

    protected PageToolbar Toolbar { get; } = new();

    [Inject]
    protected NavigationManager Navigation { get; set; }

    [Inject]
    protected IOptionAppService OptionAppService { get; set; }

    [Inject]
    protected IQuestionBankAdminAppService QuestionBankAppService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        // TODO:Waiting for update to .net 10
        QuestionTypes = //OptionAppService.GetQuestionTypes();
        new()
            {
                { 0, "SingleSelect" },
                { 1, "MultiSelect" },
                { 2, "Judge" },
                { 3, "FillInTheBlanks" }
            };

        await SearchQuestionBanks();
        await base.OnInitializedAsync();
    }

    protected virtual async Task SearchQuestionBanks(string? keyword = null)
    {
        var input = new GetQuestionBanksInput { MaxResultCount = 1000 };
        if (string.IsNullOrEmpty(keyword) == false)
        {
            input.Title = keyword;
        }
        var result = await QuestionBankAppService.GetListAsync(input);
        QuestionBanks = result.Items;
    }

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
                        await GoUpdateAsync(data.As<QuestionListDto>().Id);
                    }
                },
                new EntityAction
                {
                    Text = L["Delete"],
                    Visible = (data) => HasDeletePermission,
                    Clicked = async (data) => await DeleteAsync(data.As<QuestionListDto>().Id),
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
                    Title = L["QuestionBank"],
                    Data = nameof(QuestionListDto.QuestionBank),
                    Width = "180"
                },
                new TableColumn
                {
                    Title = L["QuestionType"],
                    Data = nameof(QuestionListDto.QuestionType),
                    ValueConverter = data=> L["QuestionType:" + data.As<QuestionListDto>().QuestionType],
                    Width = "60",
                },
                new TableColumn
                {
                    Title = L["Content"],
                    Data = nameof(QuestionListDto.Content),
                },
                new TableColumn
                {
                    Title = L["CreationTime"],
                    Data = nameof(QuestionListDto.CreationTime),
                    Width = "180",
                    Sortable = true,
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

    protected override async Task<PagedResultDto<QuestionListDto>> GetListAsync(GetQuestionsInput input)
    {
        return await AppService.GetListAsync(input);
    }

    protected virtual Task GoCreateAsync()
    {
        Navigation.NavigateTo("/questions/new", true);
        return Task.CompletedTask;
    }

    protected virtual Task GoUpdateAsync(Guid id)
    {
        // TODO: Should be remove true
        Navigation.NavigateTo("/questions/" + id, true);
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