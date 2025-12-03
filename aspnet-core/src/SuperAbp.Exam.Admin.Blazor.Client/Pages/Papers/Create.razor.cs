using AntDesign;
using Lsw.Abp.AntDesignUI;
using Lsw.Abp.AspnetCore.Components.Web.AntDesignTheme.PageToolbars;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SuperAbp.Exam.Admin.PaperManagement.Papers;
using SuperAbp.Exam.Admin.QuestionManagement.Questions;
using SuperAbp.Exam.Permissions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperAbp.Exam.Admin.Blazor.Client.Pages.Papers
{
    [Authorize(ExamPermissions.Papers.Create)]
    public partial class Create
    {
        protected PaperCreateDto Paper { get; set; } = new PaperCreateDto();
        protected List<AbpBreadcrumbItem> BreadcrumbItems = new();
        protected PageToolbar Toolbar { get; } = new();
        protected bool Loading = true;
        protected bool SubmitLoading = false;
        protected IForm CreateForm { get; set; }

        [Parameter]
        public int Mode { get; set; }

        [Inject]
        protected NavigationManager Navigation { get; set; }

        [Inject]
        protected IPaperAdminAppService PaperAppService { get; set; }

        protected override void OnInitialized()
        {
            SetBreadcrumbItems();
            Loading = false;
        }

        protected virtual void SetBreadcrumbItems()
        {
            BreadcrumbItems.Add(new AbpBreadcrumbItem(L["Menu:ExamManagement"]));
            BreadcrumbItems.Add(new AbpBreadcrumbItem(L["Papers"], "/papers"));
            BreadcrumbItems.Add(new AbpBreadcrumbItem(L["NewPaper"]));
        }

        protected virtual bool IsFormValid()
        {
            if (CreateForm == null)
            {
                return false;
            }
            return CreateForm.Validate();
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
                await PaperAppService.CreateAsync(Paper);
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