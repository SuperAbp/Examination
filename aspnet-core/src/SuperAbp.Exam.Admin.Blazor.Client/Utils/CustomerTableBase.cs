using AntDesign.TableModels;
using Localization.Resources.AbpUi;
using Lsw.Abp.AntDesignUI;
using Lsw.Abp.AntDesignUI.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using SuperAbp.Exam.Admin.Apps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.Localization;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;

namespace SuperAbp.Exam.Admin.Blazor.Client.Utils;

public abstract class CustomerTableBase<TAppService,
            TGetListOutputDto,
            TKey,
            TGetListInput,
            TListViewModel>
        : AbpComponentBase
        where TGetListOutputDto : IEntityDto<TKey>
        where TGetListInput : new()
        where TListViewModel : IEntityDto<TKey>
{
    [Inject] protected TAppService AppService { get; set; }

    protected virtual int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;

    protected int CurrentPage = 1;
    protected string CurrentSorting;
    protected int TotalCount;
    protected bool Loading = false;
    protected TGetListInput GetListInput = new();
    protected IReadOnlyList<TListViewModel> Entities = Array.Empty<TListViewModel>();

    protected virtual async Task GetEntitiesAsync()
    {
        try
        {
            Loading = true;
            await UpdateGetListInputAsync();
            // TODO: Crud AppService
            var result = await GetListAsync(GetListInput);// AppService.GetListAsync(GetListInput);
            Entities = MapToListViewModel(result.Items);
            TotalCount = (int)result.TotalCount;
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }

        Loading = false;
    }

    protected abstract Task<PagedResultDto<TGetListOutputDto>> GetListAsync(TGetListInput input);

    private IReadOnlyList<TListViewModel> MapToListViewModel(IReadOnlyList<TGetListOutputDto> dtos)
    {
        if (typeof(TGetListOutputDto) == typeof(TListViewModel))
        {
            return dtos.As<IReadOnlyList<TListViewModel>>();
        }

        return ObjectMapper.Map<IReadOnlyList<TGetListOutputDto>, List<TListViewModel>>(dtos);
    }

    protected virtual Task UpdateGetListInputAsync()
    {
        if (GetListInput is ISortedResultRequest sortedResultRequestInput)
        {
            sortedResultRequestInput.Sorting = CurrentSorting;
        }

        if (GetListInput is IPagedResultRequest pagedResultRequestInput)
        {
            pagedResultRequestInput.SkipCount = (CurrentPage - 1) * PageSize;
        }

        if (GetListInput is ILimitedResultRequest limitedResultRequestInput)
        {
            limitedResultRequestInput.MaxResultCount = PageSize;
        }

        return Task.CompletedTask;
    }

    protected virtual async Task SearchEntitiesAsync()
    {
        CurrentPage = 1;

        await GetEntitiesAsync();

        await InvokeAsync(StateHasChanged);
    }

    protected virtual async Task OnDataGridReadAsync(QueryModel<TListViewModel> e)
    {
        CurrentSorting = e.SortModel
            .Select(c => c.FieldName + (c.Sort == "descend" ? " DESC" : ""))
            .JoinAsString(",");
        CurrentPage = e.PageIndex;

        await GetEntitiesAsync();

        await InvokeAsync(StateHasChanged);
    }
}