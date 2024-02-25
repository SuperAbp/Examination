using Blazorise;
using Blazorise.DataGrid;
using SuperAbp.Exam.QuestionManagement.QuestionRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using BreadcrumbItem = Volo.Abp.BlazoriseUI.BreadcrumbItem;

namespace SuperAbp.Exam.Blazor.Pages.QuestionRepos
{
    public partial class Index
    {
        private List<TableColumn> ExamTableColumns = new List<TableColumn>();

        protected IReadOnlyList<QuestionRepoListDto> Repositories = Array.Empty<QuestionRepoListDto>();

        protected string CurrentSorting = default!;
        protected int CurrentPage = 1;
        protected int PageSize = 10;
        protected int? TotalCount = default!;
        protected PageToolbar Toolbar { get; } = new();
        protected List<BreadcrumbItem> BreadcrumbItems = new();

        protected override async Task OnInitializedAsync()
        {
            await SetTableColumnsAsync();
        }
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                SetBreadcrumbItem();
            }
        }
        protected virtual void SetBreadcrumbItem()
        {
            BreadcrumbItems.Add(new BreadcrumbItem(L["Menu:QuestionRepository"].Value));
        }

        protected virtual async Task OnDataGridReadAsync(DataGridReadDataEventArgs<QuestionRepoListDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.SortField + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;

            await GetEntitiesAsync();
        }

        protected async Task SetTableColumnsAsync()
        {
            ExamTableColumns.AddRange(
                new List<TableColumn>()
                {
                new TableColumn
                {
                    Title = L["Actions"],
                    Actions = new List<Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions.EntityAction>()
                    {
                        new Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions.EntityAction()
                        {
                            Text = L["Detail"],
                            Clicked = async (data) => {
                                GoDetail(data.As<QuestionRepoListDto>().Id);
                                await Task.CompletedTask;
                            }
                        }
                    }
                },new TableColumn
                {
                    Title = L["Title"],
                    Data = nameof(QuestionRepoListDto.Title)
                },
                        new TableColumn
                {
                    Title = L["CreationTime"],
                    Data = nameof(QuestionRepoListDto.CreationTime)
                }
            });
            await Task.CompletedTask;
        }

        protected virtual async Task GetEntitiesAsync()
        {
            var result = await QuestionRepoAppService.GetListAsync(new GetQuestionReposInput()
            {
                Sorting = CurrentSorting,
                SkipCount = (CurrentPage - 1) * PageSize,
                MaxResultCount = PageSize
            });
            Repositories = result.Items;
            TotalCount = (int?)result.TotalCount;
        }

        private void GoDetail(Guid id)
        {
            Navigation.NavigateTo($"/repository/{id}");
        }
    }
}
