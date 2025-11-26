using Localization.Resources.AbpUi;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using SuperAbp.Exam.Admin.QuestionManagement.Questions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperAbp.Exam.Admin.Blazor.Client.Pages.Questions;

public partial class SingleSelectComponent
{
    [Parameter]
    public required List<QuestionCreateOrUpdateAnswerDto> Options { get; set; } = [];

    [Parameter]
    public EventCallback<List<QuestionCreateOrUpdateAnswerDto>> OptionsChanged { get; set; }

    [Inject] protected IStringLocalizer<AbpUiResource> UiLocalizer { get; set; }
    [Inject] protected IQuestionAdminAppService QuestionAppService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await Add();
        await Add();
    }

    protected virtual async Task Add()
    {
        if (Options is null)
        {
            return;
        }
        Options.Add(new QuestionCreateOrUpdateAnswerDto());
        await OptionsChanged.InvokeAsync(Options);
    }

    protected virtual async Task ChangeAnswerAsync(QuestionCreateOrUpdateAnswerDto item, bool answer)
    {
        Options.ForEach(o => o.Right = false);
        item.Right = answer;
        await OptionsChanged.InvokeAsync(Options);
    }

    protected virtual async Task DeleteAsync(int index)
    {
        Options.RemoveAt(index);
        await OptionsChanged.InvokeAsync(Options);
    }
}