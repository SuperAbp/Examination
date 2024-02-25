using Blazorise;
using Microsoft.AspNetCore.Components;
using SuperAbp.Exam.QuestionManagement.QuestionRepos;
using SuperAbp.Exam.QuestionManagement.Questions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;

namespace SuperAbp.Exam.Blazor.Pages.QuestionRepos
{
    public partial class Detail
    {
        [Parameter]
        public Guid Id { get; set; }

        IReadOnlyList<QuestionType> _questionTypes = Array.Empty<QuestionType>();
        protected PageToolbar Toolbar { get; } = new();
        QuestionRepoDetailDto QuestionRepository;

        protected override async Task OnInitializedAsync()
        {
            QuestionRepository = await QuestionRepoAppService.GetAsync(Id);
            _questionTypes = await GetQuestionTypesAsync();

            Toolbar.AddButton("New Item", () =>
            {
                //Write your click action here
                return Task.CompletedTask;
            }, icon: IconName.Add);
        }

        private async Task<IReadOnlyList<QuestionType>> GetQuestionTypesAsync()
        {
            return (await QuestionRepoAppService.GetQuestionTypesAsync(Id)).Items;
        }
        private void StartTraining(int trainType, int? questionType = null)
        {
            var url = $"/repository/{Id}/training";
            if (trainType > 0)
            {
                url = $"/repository/{Id}/training/{trainType}";
            }
            else
            {
                if (questionType.HasValue)
                {
                    url += "?questionType=" + questionType.Value;
                }
            }
            Navigation.NavigateTo(url);
        }
        private void GoDetail()
        {
            Navigation.NavigateTo("/repository");
        }
    }
}
