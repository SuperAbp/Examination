using Microsoft.Extensions.DependencyInjection;
using SuperAbp.AuditLogging;
using SuperAbp.Exam.Admin.QuestionManagement.Questions;
using SuperAbp.MenuManagement;
using System;
using SuperAbp.Exam.QuestionManagement.Questions;
using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace SuperAbp.Exam.Admin;

[DependsOn(
    typeof(ExamDomainModule),
    typeof(AbpAccountApplicationModule),
    typeof(ExamApplicationAdminContractsModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule),
    typeof(SuperAbpMenuManagementApplicationModule),
    typeof(SuperAbpAuditLoggingApplicationModule)
    )]
public class ExamApplicationAdminModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<ExamApplicationAdminModule>();
        });
        context.Services.AddTransient(factory =>
        {
            Func<int, IQuestionAnalysis?> accessor = key =>
            {
                return QuestionType.FromValue(key).Name switch
                {
                    nameof(QuestionType.SingleSelect) => factory.GetService<SelectAnalysis>(),
                    nameof(QuestionType.MultiSelect) => factory.GetService<SelectAnalysis>(),
                    nameof(QuestionType.Judge) => factory.GetService<JudgeAnalysis>(),
                    nameof(QuestionType.FillInTheBlanks) => factory.GetService<FillInTheBlanksAnalysis>(),
                    _ => throw new ArgumentNullException($"Not Support key : {key}")
                };
            };
            return accessor;
        });
    }
}