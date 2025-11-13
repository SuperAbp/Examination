using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SuperAbp.Exam.QuestionManagement.QuestionBanks;
using SuperAbp.MenuManagement.Menus;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace SuperAbp.Exam.Data
{
    internal class ExamDataSeedContributor(ICurrentTenant currentTenant, IGuidGenerator guidGenerator, IQuestionBankRepository questionRepoRepository, IMenuRepository menuRepository) : IDataSeedContributor, ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator = guidGenerator;
        private readonly ICurrentTenant _currentTenant = currentTenant;
        private readonly IMenuRepository menuRepository = menuRepository;

        public async Task SeedAsync(DataSeedContext context)
        {
            using (_currentTenant.Change(context?.TenantId))
            {
                await CreateMenuAsync();
            }
        }

        private async Task CreateMenuAsync()
        {
            if ((await menuRepository.GetCountAsync()) > 0)
            {
                return;
            }
            List<Menu> menus = new List<Menu>();
            Guid mainMenuId = _guidGenerator.Create();
            Guid systemMenuId = _guidGenerator.Create();
            Guid permissionMenuId = _guidGenerator.Create();
            Guid examMenuId = _guidGenerator.Create();

            await menuRepository.InsertManyAsync(
            [
                new(mainMenuId)
                {
                    Name = "主导航",
                    Group = true
                },
                new(_guidGenerator.Create())
                {
                    Name = "Dashboard",
                    Icon = "dashboard",
                    ParentId = mainMenuId,
                    Sort = 0
                },
                new(systemMenuId)
                {
                    Name = "系统管理",
                    Icon = "setting",
                    ParentId = mainMenuId,
                    Sort = 50
                },
                new(permissionMenuId)
                {
                    Name = "权限管理",
                    Icon = "verified",
                    ParentId = mainMenuId,
                    Sort = 100
                },
                new(examMenuId)
                {
                    Name = "考试管理",
                    Icon = "appstore",
                    ParentId = mainMenuId,
                    Sort = 150
                },
                new Menu(_guidGenerator.Create())
                {
                    Name = "知识点",
                    ParentId = systemMenuId,
                    Permission = "Exam.KnowledgePoints.Management",
                    Route = "/sys/knowledge-point"
                },
                new Menu(_guidGenerator.Create())
                {
                    Name = "用户管理",
                    ParentId = permissionMenuId,
                    Permission = "AbpIdentity.Users",
                    Route = "/identity/user"
                },
                new Menu(_guidGenerator.Create())
                {
                    Name = "租户管理",
                    ParentId = permissionMenuId,
                    Permission = "AbpTenantManagement.Tenants",
                    Route = "/tenant-management/tenant"
                },
                new Menu(_guidGenerator.Create())
                {
                    Name = "角色管理",
                    ParentId = permissionMenuId,
                    Permission = "AbpIdentity.Roles",
                    Route = "/identity/role"
                },
                new Menu(_guidGenerator.Create())
                {
                    Name = "菜单管理",
                    ParentId = permissionMenuId,
                    Permission = "SuperAbpMenuManagement.Menu.Management",
                    Route = "/menu-management/menu"
                },
                new Menu(_guidGenerator.Create())
                {
                    Name = "题库管理",
                    ParentId = examMenuId,
                    Permission = "Exam.QuestionBanks.Management",
                    Route = "/question-management/question-bank"
                },
                new Menu(_guidGenerator.Create())
                {
                    Name = "问题管理",
                    ParentId = examMenuId,
                    Permission = "Exam.Questions",
                    Route = "/question-management/question"
                },
                new Menu(_guidGenerator.Create())
                {
                    Name = "试卷管理",
                    ParentId = examMenuId,
                    Permission = "Exam.Papers",
                    Route = "/paper-management/paper"
                },
                new Menu(_guidGenerator.Create())
                {
                    Name = "考试管理",
                    ParentId = examMenuId,
                    Permission = "Exam.Exams",
                    Route = "/exam-management/exam"
                }
            ]);
        }
    }
}