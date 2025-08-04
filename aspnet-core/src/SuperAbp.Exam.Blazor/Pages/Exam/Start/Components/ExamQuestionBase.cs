using Microsoft.AspNetCore.Components;
using Blazored.LocalStorage;
using SuperAbp.Exam.Blazor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperAbp.Exam.Blazor.Pages.Exam.Start.Components
{
    public abstract class ExamQuestionBase : ComponentBase
    {
        [Inject]
        protected ILocalStorageService LocalStorage { get; set; }

        [Parameter]
        public string QuestionAnswerStorageKey { get; set; }

        [Parameter]
        public EventCallback<Guid> OnAnswered { get; set; }

        protected async Task SaveAnswerAsync(QuestionAnswerItem item)
        {
            var answerItems = await LocalStorage.GetItemAsync<List<QuestionAnswerItem>>(QuestionAnswerStorageKey) ?? new List<QuestionAnswerItem>();
            answerItems = answerItems.Where(x => x.QuestionId != item.QuestionId).ToList();
            answerItems.Add(item);
            await LocalStorage.SetItemAsync(QuestionAnswerStorageKey, answerItems);

            if (OnAnswered.HasDelegate)
            {
                await OnAnswered.InvokeAsync(item.QuestionId);
            }
        }
    }
}