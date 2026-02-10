using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SuperAbp.Exam.Announcements;
using SuperAbp.Exam.ExamManagement.UserExams;
using SuperAbp.Exam.PaperManagement.Papers;
using SuperAbp.Exam.QuestionManagement.Questions;
using SuperAbp.MenuManagement.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace SuperAbp.Exam.EntityFrameworkCore;

[DependsOn(
    typeof(ExamDomainModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(AbpOpenIddictEntityFrameworkCoreModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpEntityFrameworkCoreSqlServerModule),
    typeof(AbpBackgroundJobsEntityFrameworkCoreModule),
    typeof(AbpAuditLoggingEntityFrameworkCoreModule),
    typeof(AbpTenantManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(SuperAbpMenuManagementEntityFrameworkCoreModule)
    )]
public class ExamEntityFrameworkCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        ExamEfCoreEntityExtensionMappings.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<ExamDbContext>(options =>
        {
            /* Remove "includeAllEntities: true" to create
             * default repositories only for aggregate roots */
            options.AddDefaultRepositories(includeAllEntities: true);
        });

        Configure<AbpDbContextOptions>(options =>
        {
            /* The main point to change your DBMS.
             * See also ExamMigrationsDbContextFactory for EF Core tooling. */
            options.UseSqlServer();
        });
        Configure<AbpEntityOptions>(options =>
        {
            options.Entity<Paper>(option =>
            {
                option.DefaultWithDetailsFunc = query => query
                .Include(o => o.PaperSections).ThenInclude(s => s.PaperQuestions)
                .Include(o => o.PaperSections).ThenInclude(s => s.PaperQuestionRules);
            });
            options.Entity<Question>(option =>
            {
                option.DefaultWithDetailsFunc = query => query.Include(o => o.Options);
            });
            options.Entity<UserExam>(option =>
            {
                option.DefaultWithDetailsFunc = query => query.Include(o => o.Sections).ThenInclude(o => o.Questions).ThenInclude(q => q.QuestionReviews);
            });
            options.Entity<Announcement>(option =>
            {
                option.DefaultWithDetailsFunc = query => query.Include(o => o.Category);
            });
        });
    }
}