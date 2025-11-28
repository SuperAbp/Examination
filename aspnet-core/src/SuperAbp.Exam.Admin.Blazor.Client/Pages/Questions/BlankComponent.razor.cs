using Microsoft.AspNetCore.Components;
using SuperAbp.Exam.Admin.QuestionManagement.Questions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperAbp.Exam.Admin.Blazor.Client.Pages.Questions
{
    public partial class BlankComponent
    {
        [Parameter]
        public List<QuestionCreateOrUpdateAnswerDto> Options { get; set; } = [];

        [Parameter]
        public EventCallback<List<QuestionCreateOrUpdateAnswerDto>> OptionsChanged { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (Options.Count == 0)
            {
                await AddAsync();
            }
        }

        protected virtual async Task AddAsync()
        {
            if (Options is null)
            {
                return;
            }
            QuestionCreateOrUpdateAnswerDto newOption = new() { Sort = 0, Right = true };
            Options.Add(newOption);
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
}