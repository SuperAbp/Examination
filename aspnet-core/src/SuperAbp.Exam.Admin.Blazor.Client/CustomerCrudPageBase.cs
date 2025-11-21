using AntDesign;
using AntDesign.TableModels;
using AutoMapper.Internal.Mappers;
using JetBrains.Annotations;
using Localization.Resources.AbpUi;
using Lsw.Abp.AntDesignUI;
using Lsw.Abp.AntDesignUI.Components;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
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

namespace SuperAbp.Exam.Admin.Blazor.Client
{
    public abstract class CustomerCrudPageBase<TAppService,
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

        [Inject] protected IStringLocalizer<AbpUiResource> UiLocalizer { get; set; }

        [Inject] protected IAbpEnumLocalizer AbpEnumLocalizer { get; set; }

        protected virtual int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;

        protected int CurrentPage = 1;
        protected string CurrentSorting;
        protected int TotalCount;
        protected bool Loading = false;
        protected TGetListInput GetListInput = new();
        protected IReadOnlyList<TListViewModel> Entities = Array.Empty<TListViewModel>();
        protected TKey EditingEntityId;
        protected Modal CreateModal;
        protected bool CreateModalVisible;
        protected Modal EditModal;
        protected bool EditModalVisible;
        protected List<AbpBreadcrumbItem> BreadcrumbItems = new();
        protected TableEntityActionsColumn<TListViewModel> EntityActionsColumn;
        protected EntityActionDictionary EntityActions { get; set; }
        protected TableColumnDictionary TableColumns { get; set; }

        protected string CreatePolicyName { get; set; }
        protected string UpdatePolicyName { get; set; }
        protected string DeletePolicyName { get; set; }

        public bool HasCreatePermission { get; set; }
        public bool HasUpdatePermission { get; set; }
        public bool HasDeletePermission { get; set; }

        protected CustomerCrudPageBase()
        {
            TableColumns = new TableColumnDictionary();
            EntityActions = new EntityActionDictionary();
        }

        protected override async Task OnInitializedAsync()
        {
            await SetPermissionsAsync();
            await SetEntityActionsAsync();
            await SetTableColumnsAsync();
            await SetToolbarItemsAsync();
            await SetBreadcrumbItemsAsync();
            await InvokeAsync(StateHasChanged);
        }

        protected virtual async Task SetPermissionsAsync()
        {
            if (CreatePolicyName != null)
            {
                HasCreatePermission = await AuthorizationService.IsGrantedAsync(CreatePolicyName);
            }

            if (UpdatePolicyName != null)
            {
                HasUpdatePermission = await AuthorizationService.IsGrantedAsync(UpdatePolicyName);
            }

            if (DeletePolicyName != null)
            {
                HasDeletePermission = await AuthorizationService.IsGrantedAsync(DeletePolicyName);
            }
        }

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

        protected virtual async Task CheckCreatePolicyAsync()
        {
            await CheckPolicyAsync(CreatePolicyName);
        }

        protected virtual async Task CheckUpdatePolicyAsync()
        {
            await CheckPolicyAsync(UpdatePolicyName);
        }

        protected virtual async Task CheckDeletePolicyAsync()
        {
            await CheckPolicyAsync(DeletePolicyName);
        }

        /// <summary>
        /// Calls IAuthorizationService.CheckAsync for the given <paramref name="policyName"/>.
        /// Throws <see cref="AbpAuthorizationException"/> if given policy was not granted for the current user.
        ///
        /// Does nothing if <paramref name="policyName"/> is null or empty.
        /// </summary>
        /// <param name="policyName">A policy name to check</param>
        protected virtual async Task CheckPolicyAsync([CanBeNull] string policyName)
        {
            if (string.IsNullOrEmpty(policyName))
            {
                return;
            }

            await AuthorizationService.CheckAsync(policyName);
        }

        protected virtual ValueTask SetBreadcrumbItemsAsync()
        {
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetEntityActionsAsync()
        {
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetTableColumnsAsync()
        {
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            return ValueTask.CompletedTask;
        }

        protected virtual IEnumerable<TableColumn> GetExtensionTableColumns(string moduleName, string entityType)
        {
            var properties = ModuleExtensionConfigurationHelper.GetPropertyConfigurations(moduleName, entityType);
            foreach (var propertyInfo in properties)
            {
                if (propertyInfo.IsAvailableToClients && propertyInfo.UI.OnTable.IsVisible)
                {
                    if (propertyInfo.Name.EndsWith("_Text"))
                    {
                        var lookupPropertyName = propertyInfo.Name.RemovePostFix("_Text");
                        var lookupPropertyDefinition = properties.SingleOrDefault(t => t.Name == lookupPropertyName);
                        yield return new TableColumn
                        {
                            Title = lookupPropertyDefinition.GetLocalizedDisplayName(StringLocalizerFactory),
                            Data = $"ExtraProperties[{propertyInfo.Name}]"
                        };
                    }
                    else
                    {
                        var column = new TableColumn
                        {
                            Title = propertyInfo.GetLocalizedDisplayName(StringLocalizerFactory),
                            Data = $"ExtraProperties[{propertyInfo.Name}]"
                        };

                        if (propertyInfo.IsDate() || propertyInfo.IsDateTime())
                        {
                            column.DisplayFormat = propertyInfo.GetDateEditInputFormatOrNull();
                        }

                        if (propertyInfo.Type.IsEnum)
                        {
                            column.ValueConverter = (val) =>
                                AbpEnumLocalizer.GetString(propertyInfo.Type, val.As<ExtensibleObject>().ExtraProperties[propertyInfo.Name]);
                        }

                        yield return column;
                    }
                }
            }
        }
    }
}