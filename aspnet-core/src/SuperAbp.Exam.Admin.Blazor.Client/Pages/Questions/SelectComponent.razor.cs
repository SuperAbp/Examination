using Microsoft.AspNetCore.Components;
using SuperAbp.Exam.Admin.QuestionManagement.Questions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperAbp.Exam.Admin.Blazor.Client.Pages.Questions;

public partial class SelectComponent
{
    [Parameter]
    public bool Single { get; set; } = false;

    [Parameter]
    public List<QuestionCreateOrUpdateAnswerDto> Options { get; set; } = [];

    [Parameter]
    public EventCallback<List<QuestionCreateOrUpdateAnswerDto>> OptionsChanged { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (Options.Count == 0)
        {
            await AddAsync(true);
            await AddAsync();
        }
    }

    private async Task AddAsync() => await AddAsync(false);

    protected virtual async Task AddAsync(bool right = false)
    {
        if (Options is null)
        {
            return;
        }
        var newOption = new QuestionCreateOrUpdateAnswerDto { Sort = Options.Count + 1, Right = right };
        Options.Add(newOption);
        await UpdateValueAsync();
    }

    protected virtual async Task ChangeAnswerAsync(QuestionCreateOrUpdateAnswerDto item, bool answer)
    {
        Options.ForEach(o => o.Right = false);
        item.Right = answer;
        await UpdateValueAsync();
    }

    protected virtual async Task DeleteAsync(int index)
    {
        Options.RemoveAt(index);
        await OptionsChanged.InvokeAsync(Options);
    }

    protected virtual async Task UpdateValueAsync()
    {
        await OptionsChanged.InvokeAsync(Options);
    }
}