using AntDesign;
using Lsw.Abp.AntDesignUI;
using Lsw.Abp.AspnetCore.Components.Web.AntDesignTheme.PageToolbars;
using Microsoft.AspNetCore.Components;
using SuperAbp.Exam.Admin.KnowledgePoints;
using SuperAbp.Exam.Admin.Options;
using SuperAbp.Exam.Admin.QuestionManagement.QuestionBanks;
using SuperAbp.Exam.Admin.QuestionManagement.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperAbp.Exam.Admin.Blazor.Client.Pages.Questions
{
    public partial class Create
    {
        protected bool Loading = true;
        protected bool SubmitLoading = false;
        protected Dictionary<int, string> QuestionTypes;
        protected List<AbpBreadcrumbItem> BreadcrumbItems = new();
        protected PageToolbar Toolbar { get; } = new();
        protected bool LoadingQuestionBank = false;
        protected IForm CreateForm { get; set; }

        protected QuestionCreateDto Question { get; set; } = new QuestionCreateDto();

        protected IReadOnlyList<QuestionBankListDto> QuestionBanks { get; set; }
        protected IReadOnlyList<KnowledgePointNodeDto> KnowledgePoints { get; set; }

        [Inject]
        protected NavigationManager Navigation { get; set; }

        [Inject]
        protected IOptionAppService OptionAppService { get; set; }

        [Inject]
        protected IQuestionAdminAppService QuestionAppService { get; set; }

        [Inject]
        protected IQuestionBankAdminAppService QuestionBankAppService { get; set; }

        [Inject]
        protected IKnowledgePointAdminAppService knowledgePointAppService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            QuestionTypes = //OptionAppService.GetQuestionTypes();
            new()
            {
                { 0, "SingleSelect" },
                { 1, "MultiSelect" },
                { 2, "Judge" },
                { 3, "FillInTheBlanks" }
            };

            await SearchQuestionBanks();
            await SearchKnowledgePoints();
            SetBreadcrumbItems();
            await base.OnInitializedAsync();
            Loading = false;
            await InvokeAsync(StateHasChanged);
        }

        protected virtual async Task SearchKnowledgePoints()
        {
            var result = await knowledgePointAppService.GetAllAsync(new GetKnowledgePointsInput());
            KnowledgePoints = result.Items;
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
            Question.QuestionBankId = QuestionBanks.FirstOrDefault()?.Id ?? Guid.Empty;
        }

        protected virtual void SetBreadcrumbItems()
        {
            BreadcrumbItems.Add(new AbpBreadcrumbItem(L["Menu:ExamManagement"]));
            BreadcrumbItems.Add(new AbpBreadcrumbItem(L["Questions"], "/questions"));
            BreadcrumbItems.Add(new AbpBreadcrumbItem(L["NewQuestion"]));
        }

        protected virtual async Task Save()
        {
            try
            {
                var validate = true;
                if (CreateForm != null)
                {
                    validate = CreateForm.Validate();
                }
                if (!validate)
                {
                    return;
                }

                SubmitLoading = true;
                StateHasChanged();
                await QuestionAppService.CreateAsync(Question);
            }
            finally
            {
                SubmitLoading = false;
            }
            Back();
        }

        protected virtual void Back()
        {
            Navigation.NavigateTo("/questions");
        }
    }
}