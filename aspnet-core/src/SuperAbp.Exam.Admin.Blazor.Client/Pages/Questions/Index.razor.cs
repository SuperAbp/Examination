using AntDesign;
using Lsw.Abp.AntDesignUI;
using Lsw.Abp.AspnetCore.Components.Web.AntDesignTheme.PageToolbars;
using Lsw.Abp.IdentityManagement.Blazor.AntDesignUI;
using Microsoft.AspNetCore.Authorization;
using SuperAbp.Exam.Admin.Blazor.Client.Pages.QuestionBanks;
using SuperAbp.Exam.Admin.QuestionManagement.QuestionBanks;
using SuperAbp.Exam.Admin.QuestionManagement.Questions;
using SuperAbp.Exam.Localization;
using SuperAbp.Exam.Permissions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.Identity.Localization;
using Volo.Abp.ObjectExtending;

namespace SuperAbp.Exam.Admin.Blazor.Client.Pages.Questions
{
    [Authorize(ExamPermissions.Questions.Management)]
    public partial class Index
    {
        public Index()
        {
            ObjectMapperContext = typeof(ExamBlazorClientModule);
            LocalizationResource = typeof(ExamResource);

            CreatePolicyName = ExamPermissions.Questions.Create;
            UpdatePolicyName = ExamPermissions.Questions.Update;
            DeletePolicyName = ExamPermissions.Questions.Delete;
        }

        protected List<TableColumn> QuesionTableColumns => TableColumns.Get<Index>();

        protected IForm SearchForm { get; set; }

        protected PageToolbar Toolbar { get; } = new();
        protected CreateModal CreateModal { get; set; }
        protected UpdateModal UpdateModal { get; set; }

        protected override ValueTask SetEntityActionsAsync()
        {
            EntityActions
                .Get<Index>()
                .AddRange(
                [
                        new EntityAction
                    {
                        Primary = true,
                        Text = L["Edit"],
                        Visible = (data) => HasUpdatePermission,
                        Clicked = async (data) =>
                        {
                            await OpenEditModalAsync(data.As<QuestionBankListDto>().Id);
                        }
                    },
                    new EntityAction
                    {
                        Text = L["Delete"],
                        Visible = (data) => HasDeletePermission,
                        Clicked = async (data) => await DeleteAsync(data.As<QuestionBankListDto>().Id),
                        ConfirmationMessage = (data) => L["ItemWillBeDeletedMessage"]
                    }
                ]);

            return base.SetEntityActionsAsync();
        }

        protected override ValueTask SetTableColumnsAsync()
        {
            QuesionTableColumns
                .AddRange(
                [
                    new TableColumn
                    {
                        Title = L["QuestionBank"],
                        Data = nameof(QuestionListDto.QuestionBank),
                        Width = "180"
                    },
                    new TableColumn
                    {
                        Title = L["QuestionType"],
                        Data = nameof(QuestionListDto.QuestionType),
                        ValueConverter = data=> L["QuestionType:" + data.As<QuestionListDto>().QuestionType],
                        Width = "60",
                    },
                    new TableColumn
                    {
                        Title = L["Content"],
                        Data = nameof(QuestionListDto.Content),
                    },
                    new TableColumn
                    {
                        Title = L["CreationTime"],
                        Data = nameof(QuestionListDto.CreationTime),
                    }
                ]);

            QuesionTableColumns.AddRange(GetExtensionTableColumns(IdentityModuleExtensionConsts.ModuleName,
                IdentityModuleExtensionConsts.EntityNames.Role));

            QuesionTableColumns.Add(new TableColumn
            {
                Title = L["Actions"],
                Actions = EntityActions.Get<Index>()
            });

            return base.SetTableColumnsAsync();
        }

        protected override async ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["NewQuestionBank"], OpenCreateModalAsync,
                IconType.Outline.Plus,
                requiredPolicyName: ExamPermissions.QuestionBanks.Create);
            await base.SetToolbarItemsAsync();
        }

        protected override async ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new AbpBreadcrumbItem(L["Menu:ExamManagement"]));
            BreadcrumbItems.Add(new AbpBreadcrumbItem(L["QuestionBanks"]));
            await base.SetBreadcrumbItemsAsync();
        }

        protected virtual async Task ClearSearchAsync()
        {
            SearchForm.Reset();
            await SearchEntitiesAsync();
        }

        protected override async Task<PagedResultDto<QuestionListDto>> GetListAsync(GetQuestionsInput input)
        {
            return await AppService.GetListAsync(input);
        }

        protected virtual async Task OpenCreateModalAsync()
        {
            await CreateModal.OpenAsync();
        }

        protected virtual async Task OpenEditModalAsync(Guid id)
        {
            await UpdateModal.OpenAsync(id);
        }

        protected async Task OnSaveSuccessAsync()
        {
            // TODO: Use `ModalService` to create modal;
            await SearchEntitiesAsync();
        }

        protected async Task DeleteAsync(Guid id)
        {
            await AppService.DeleteAsync(id);
            await SearchEntitiesAsync();
        }
    }
}