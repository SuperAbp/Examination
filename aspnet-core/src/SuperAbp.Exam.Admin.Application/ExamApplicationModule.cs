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
    typeof(ExamApplicationContractsModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule),
    typeof(SuperAbpMenuManagementApplicationModule),
    typeof(SuperAbpAuditLoggingApplicationModule)
    )]
public class ExamApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<ExamApplicationModule>();
        });
        context.Services.AddTransient(factory =>
        {
            Func<QuestionType, IQuestionAnalysis> accessor = key =>
            {
                return key switch
                {
                    QuestionType.SingleSelect => factory.GetService<SelectAnalysis>(),
                    QuestionType.MultiSelect => factory.GetService<SelectAnalysis>(),
                    QuestionType.Judge => factory.GetService<JudgeAnalysis>(),
                    QuestionType.FillInTheBlanks => factory.GetService<FillInTheBlanksAnalysis>(),
                    _ => throw new ArgumentNullException($"Not Support key : {key}")
                };
            };
            return accessor;
        });
    }
}