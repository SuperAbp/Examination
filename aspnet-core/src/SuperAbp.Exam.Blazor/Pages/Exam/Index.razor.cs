using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using SuperAbp.Exam.ExamManagement.Exams;
using SuperAbp.Exam.ExamManagement.UserExams;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using BreadcrumbItem = Volo.Abp.BlazoriseUI.BreadcrumbItem;

namespace SuperAbp.Exam.Blazor.Pages.Exam;

public partial class Index
{

    private List<TableColumn> ExamTableColumns = new List<TableColumn>();

    protected IReadOnlyList<ExamListDto> Exams = Array.Empty<ExamListDto>();

    protected string CurrentSorting = default!;
    protected int CurrentPage = 1;
    protected int PageSize = 10;
    protected int? TotalCount = default!;
    protected PageToolbar Toolbar { get; } = new();
    protected List<BreadcrumbItem> BreadcrumbItems = new ();

    protected override async Task OnInitializedAsync()
    {
        await SetTableColumnsAsync();
    }
    protected override  void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            SetBreadcrumbItem();
        }
    }
    protected virtual void SetBreadcrumbItem()
    {
        BreadcrumbItems.Add(new BreadcrumbItem(L["Menu:MyExam"].Value));
    }

    protected virtual async Task OnDataGridReadAsync(DataGridReadDataEventArgs<ExamListDto> e)
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
                        Text = L["BeganExam"],
                        Visible = (data) =>
                        {
                            var targetData = data.As<ExamListDto>();
                            return targetData.StartTime.HasValue && targetData.EndTime.HasValue
                            && (DateTime.Now > targetData.StartTime.Value && DateTime.Now < targetData.EndTime.Value);
                        },
                        Clicked = async (data) => {
                            await BeganAsync(data.As<ExamListDto>().Id);
                        }
                    }
                }
            },new TableColumn
        {
            Title = L["Name"],
            Data = nameof(ExamListDto.Name)
        },
                new TableColumn
        {
            Title = L["Score"],
            Data = nameof(ExamListDto.Score)
        },
                new TableColumn
        {
            Title = L["TotalTime"],
            Data = nameof(ExamListDto.TotalTime)
        },new TableColumn
        {
            Title = L["ExamTime"],
            Data = nameof(ExamListDto.StartTime)
        }
            });
        await Task.CompletedTask;
    }

    protected virtual async Task GetEntitiesAsync()
    {
        var result = await ExamAppService.GetListAsync(new GetExamsInput()
        {
            Sorting = CurrentSorting,
            SkipCount = (CurrentPage - 1) * PageSize,
            MaxResultCount = PageSize
        });
        Exams = result.Items;
        TotalCount = (int?)result.TotalCount;
    }
    private async Task BeganAsync(Guid id)
    {
        var dto = await UserExamAppService.CreateAsync(new UserExamCreateDto() { ExamId = id });
        Navigation.NavigateTo($"/Exam/Start/{dto.Id}");
    }
}
