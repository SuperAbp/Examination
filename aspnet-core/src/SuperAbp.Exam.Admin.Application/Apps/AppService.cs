using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Identity;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;
using SuperAbp.MenuManagement.Menus;

namespace SuperAbp.Exam.Admin.Apps;

[Authorize]
public class AppService : ExamAppService, IAppService
{
    private readonly IMenuRepository _menuRepository;
    private readonly ISettingManager _settingManager;

    public AppService(
        IMenuRepository menuRepository,
        ISettingManager settingManager,
        IdentityUserManager userManager,
        IConfiguration configuration)
    {
        _menuRepository = menuRepository;
        _settingManager = settingManager;
    }

    public async Task<List<SettingValue>> GetSettings()
    {
        return await _settingManager.GetAllGlobalAsync();
    }

    public async Task<List<AppDataListDto>> GetDataAsync()
    {
        var menus = (await _menuRepository.GetListAsync())
            .OrderBy(m => m.Sort)
            .ToList();
        List<AppDataListDto> resultMenus = new List<AppDataListDto>();
        foreach (var menuDto in menus.Where(m => !m.ParentId.HasValue))
        {
            var children = await GetChildAsync(menuDto.Id, menus);
            if (children.Count == 0 && menus.Any(m => m.ParentId == menuDto.Id))
            {
                continue;
            }
            resultMenus.Add(new AppDataListDto
            {
                Group = menuDto.Group,
                HideInBreadcrumb = menuDto.HideInBreadcrumb,
                Key = menuDto.Key,
                Text = menuDto.Name,
                Icon = menuDto.Icon,
                Link = menuDto.Route,
                Children = children
            });
        }
        return resultMenus;
    }

    private async Task<List<AppDataListDto>> GetChildAsync(Guid parentId, List<Menu> menus)
    {
        List<AppDataListDto> resultMenus = new List<AppDataListDto>();
        foreach (var menuDto in menus.Where(m => m.ParentId == parentId))
        {
            var children = await GetChildAsync(menuDto.Id, menus);
            if (children.Count == 0 && menus.Any(m => m.ParentId == menuDto.Id))
            {
                continue;
            }
            if (menuDto.Permission.IsNullOrWhiteSpace())
            {
                resultMenus.Add(new AppDataListDto
                {
                    Group = menuDto.Group,
                    HideInBreadcrumb = menuDto.HideInBreadcrumb,
                    Key = menuDto.Key,
                    Text = menuDto.Name,
                    Icon = menuDto.Icon,
                    Link = menuDto.Route,
                    Children = children
                });
            }
            else
            {
                if (await AuthorizationService.IsGrantedAsync(menuDto.Permission))
                {
                    AppDataListDto dto = new AppDataListDto
                    {
                        Group = menuDto.Group,
                        HideInBreadcrumb = menuDto.HideInBreadcrumb,
                        Key = menuDto.Key,
                        Text = menuDto.Name,
                        Icon = menuDto.Icon,
                        Link = menuDto.Route,
                        Children = children
                    };
                    resultMenus.Add(dto);
                }
            }
        }

        return resultMenus;
    }
}