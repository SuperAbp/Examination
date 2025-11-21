using AntDesign;
using Lsw.Abp.AntDesignUI;
using Lsw.Abp.AspnetCore.Components.Web.AntDesignTheme.PageToolbars;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using SuperAbp.Exam.Admin.QuestionManagement.QuestionBanks;
using SuperAbp.Exam.Permissions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.Blazor.Client.Pages.QuestionBanks;

[Authorize]
public partial class Index
{
    protected int PageIndex = 0;
    protected int PageSize = 10;
    protected IReadOnlyList<QuestionBankListDto> QuestionBanks { get; set; }
    protected int TotalCount { get; set; }
    protected bool Loading { get; set; }
    private ModalRef _modalRef;
    protected IForm SearchForm { get; set; }
    protected GetQuestionBanksInput GetQuestionBanksInput { get; set; }
    protected UpdateModal UpdateModal { get; set; }
    protected CreateModal CreateModal { get; set; }
    protected PageToolbar Toolbar { get; } = new();
    protected List<AbpBreadcrumbItem> BreadcrumbItems = new();

    protected bool HasCreatePermission { get; set; }
    protected bool HasUpdatePermission { get; set; }
    protected bool HasDeletePermission { get; set; }

    [Inject]
    protected IQuestionBankAdminAppService QuestionBankAppService { get; set; }

    [Inject]
    protected ModalService ModalService { get; set; }

    [Inject]
    protected INotificationService NotificationService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        GetQuestionBanksInput = new GetQuestionBanksInput
        {
            MaxResultCount = PageSize,
            SkipCount = PageIndex * PageSize
        };
        SetToolbarItems();
        SetBreadcrumbItems();
        await SetPermissionsAsync();
        await GetEntitiesAsync();
    }

    protected virtual async Task SearchEntitiesAsync()
    {
        PageIndex = 0;
        await GetEntitiesAsync();
        await InvokeAsync(StateHasChanged);
    }

    protected virtual async Task ClearSearchAsync()
    {
        SearchForm.Reset();
        await SearchEntitiesAsync();
    }

    protected virtual async Task GetEntitiesAsync()
    {
        Loading = true;

        try
        {
            var result = await QuestionBankAppService
                .GetListAsync(GetQuestionBanksInput);
            QuestionBanks = result.Items;
            TotalCount = (int)result.TotalCount;
        }
        finally
        {
            Loading = false;
        }
    }

    protected virtual async Task SetPermissionsAsync()
    {
        HasUpdatePermission = await AuthorizationService.IsGrantedAsync(ExamPermissions.QuestionBanks.Update);
        HasDeletePermission = await AuthorizationService.IsGrantedAsync(ExamPermissions.QuestionBanks.Delete);
    }

    protected virtual void SetToolbarItems()
    {
        Toolbar.AddButton(L["NewQuestionBank"], OpenCreateModalAsync,
            IconType.Outline.Plus,
            requiredPolicyName: ExamPermissions.QuestionBanks.Create);
    }

    protected virtual void SetBreadcrumbItems()
    {
        BreadcrumbItems.Add(new AbpBreadcrumbItem(L["Menu:ExamManagement"]));
        BreadcrumbItems.Add(new AbpBreadcrumbItem(L["QuestionBanks"]));
    }

    private async Task OpenCreateModalAsync()
    {
        await CreateModal.OpenAsync();
    }

    protected virtual async Task OpenEditModalAsync(Guid id)
    {
        await UpdateModal.OpenAsync(id);
    }

    private async Task OnSaveSuccessAsync()
    {
        // TODO: Use `ModalService` to create modal;
        await GetEntitiesAsync();
        await InvokeAsync(StateHasChanged);
    }

    protected async Task DeleteAsync()
    {
        Console.WriteLine("111");
        await NotificationService.Info(new NotificationConfig
        {
            Message = "Info",
            Description = "This is an info notification.",
            Duration = 3
        });
        Console.WriteLine("222");
    }
}