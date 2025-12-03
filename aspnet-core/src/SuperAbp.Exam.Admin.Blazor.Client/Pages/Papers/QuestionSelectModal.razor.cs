using AntDesign;
using AntDesign.TableModels;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SuperAbp.Exam.Admin.QuestionManagement.QuestionBanks;
using SuperAbp.Exam.Admin.QuestionManagement.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.ExceptionHandling;
using Volo.Abp.AspNetCore.Components.Web.ExceptionHandling;

namespace SuperAbp.Exam.Admin.Blazor.Client.Pages.Papers;

public partial class QuestionSelectModal
{
    [Inject]
    protected IQuestionAdminAppService QuestionAppService { get; set; }

    [Inject]
    protected IQuestionBankAdminAppService QuestionBankAppService { get; set; }

    protected virtual int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;

    protected int CurrentPage = 1;
    protected string CurrentSorting;
    protected int TotalCount;
    protected bool Loading = false;
    protected GetQuestionsInput GetListInput = new();
    protected IReadOnlyList<QuestionListDto> Entities = Array.Empty<QuestionListDto>();
    protected IForm SearchForm { get; set; }

    public bool Visible { get; set; }
    protected Dictionary<int, string> QuestionTypes { get; set; }
    protected IReadOnlyList<QuestionBankListDto> QuestionBanks { get; set; }
    protected List<QuestionListDto> Questions { get; set; } = [];

    protected IEnumerable<QuestionListDto> SelectedQuestions { get; set; }

    protected string SearchContent { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        GetListInput.ExcludeIds = base.Options.ToList();
        QuestionTypes = //OptionAppService.GetQuestionTypes();
        new()
            {
                { 0, "SingleSelect" },
                { 1, "MultiSelect" },
                { 2, "Judge" },
                { 3, "FillInTheBlanks" }
            };
        await SearchQuestionBanks();
    }

    protected virtual async Task GetEntitiesAsync()
    {
        Loading = true;
        await UpdateGetListInputAsync();
        var result = await QuestionAppService.GetListAsync(GetListInput);
        Entities = result.Items;
        TotalCount = (int)result.TotalCount;

        Loading = false;
    }

    protected virtual Task UpdateGetListInputAsync()
    {
        if (GetListInput is ISortedResultRequest sortedResultRequestInput)
        {
            sortedResultRequestInput.Sorting = CurrentSorting;
        }

        if (GetListInput is IPagedResultRequest pagedResultRequestInput)
        {
            pagedResultRequestInput.SkipCount = (CurrentPage - 1) * PageSize;
        }

        if (GetListInput is ILimitedResultRequest limitedResultRequestInput)
        {
            limitedResultRequestInput.MaxResultCount = PageSize;
        }

        return Task.CompletedTask;
    }

    protected virtual async Task SearchEntitiesAsync()
    {
        CurrentPage = 1;

        await GetEntitiesAsync();

        await InvokeAsync(StateHasChanged);
    }

    protected virtual async Task OnDataGridReadAsync(QueryModel<QuestionListDto> e)
    {
        CurrentSorting = e.SortModel
            .Select(c => c.FieldName + (c.Sort == "descend" ? " DESC" : ""))
            .JoinAsString(",");
        CurrentPage = e.PageIndex;

        await GetEntitiesAsync();

        await InvokeAsync(StateHasChanged);
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

    protected virtual async Task ClearSearchAsync()
    {
        SearchForm.Reset();
        await SearchEntitiesAsync();
    }

    public override Task OnFeedbackOkAsync(ModalClosingEventArgs args)
    {
        base.OkCancelRefWithResult.OnOk(SelectedQuestions.Select(q => q.Id));
        return base.OnFeedbackOkAsync(args);
    }
}