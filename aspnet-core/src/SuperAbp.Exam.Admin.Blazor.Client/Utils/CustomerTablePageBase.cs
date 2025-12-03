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
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.Localization;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;

namespace SuperAbp.Exam.Admin.Blazor.Client.Utils
{
    public abstract class CustomerTablePageBase<TAppService,
            TGetListOutputDto,
            TKey,
            TGetListInput,
            TListViewModel>
        : CustomerTableBase<TAppService,
            TGetListOutputDto,
            TKey,
            TGetListInput,
            TListViewModel>
        where TGetListOutputDto : IEntityDto<TKey>
        where TGetListInput : new()
        where TListViewModel : IEntityDto<TKey>
    {
        [Inject] protected IStringLocalizer<AbpUiResource> UiLocalizer { get; set; }

        [Inject] protected IAbpEnumLocalizer AbpEnumLocalizer { get; set; }

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

        protected CustomerTablePageBase()
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