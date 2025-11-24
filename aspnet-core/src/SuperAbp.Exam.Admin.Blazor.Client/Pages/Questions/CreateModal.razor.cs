using AntDesign;
using Lsw.Abp.AntDesignUI;
using Lsw.Abp.AspnetCore.Components.Web.AntDesignTheme.PageToolbars;
using Microsoft.AspNetCore.Components;
using SuperAbp.Exam.Admin.KnowledgePoints;
using SuperAbp.Exam.Admin.Options;
using SuperAbp.Exam.Admin.QuestionManagement.QuestionBanks;
using SuperAbp.Exam.Admin.QuestionManagement.Questions;
using SuperAbp.Exam.QuestionManagement.Questions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperAbp.Exam.Admin.Blazor.Client.Pages.Questions
{
    public partial class CreateModal
    {
        protected Dictionary<int, string> QuestionTypes;
        protected List<AbpBreadcrumbItem> BreadcrumbItems = new();
        protected PageToolbar Toolbar { get; } = new();
        protected bool LoadingQuestionBank = false;
        protected IForm CreateForm { get; set; }

        protected QuestionCreateDto Question { get; set; }

        protected IReadOnlyList<QuestionBankListDto> QuestionBanks { get; set; }
        protected IReadOnlyList<KnowledgePointNodeDto> KnowledgePoints { get; set; }

        [Inject]
        protected IOptionAppService OptionAppService { get; set; }

        [Inject]
        protected IQuestionBankAdminAppService QuestionBankAppService { get; set; }

        [Inject]
        protected IKnowledgePointAdminAppService knowledgePointAppService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            QuestionTypes = new()
            {
                { 0, "SingleSelect" },
                { 1, "MultiSelect" },
                { 2, "Judge" },
                { 3, "FillInTheBlanks" }
            };
            await SearchQuestionBanksAsync();
            await SearchKnowledgePointsAsync();
            SetBreadcrumbItems();
            await base.OnInitializedAsync();
        }

        protected virtual async Task SearchKnowledgePointsAsync()
        {
            var result = await knowledgePointAppService.GetAllAsync(new GetKnowledgePointsInput());
            KnowledgePoints = result.Items;
        }

        protected virtual async Task SearchQuestionBanksAsync(string? keyword = null)
        {
            var input = new GetQuestionBanksInput { MaxResultCount = 1000 };
            if (string.IsNullOrEmpty(keyword) == false)
            {
                input.Title = keyword;
            }
            var result = await QuestionBankAppService.GetListAsync(input);
            QuestionBanks = result.Items;
        }

        protected void SetBreadcrumbItems()
        {
            BreadcrumbItems.Add(new AbpBreadcrumbItem(L["Menu:ExamManagement"]));
            BreadcrumbItems.Add(new AbpBreadcrumbItem(L["Questions"]));
            BreadcrumbItems.Add(new AbpBreadcrumbItem(L["NewQuestion"]));
        }
    }
}