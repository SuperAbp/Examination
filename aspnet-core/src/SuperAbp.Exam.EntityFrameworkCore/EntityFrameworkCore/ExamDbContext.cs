using SuperAbp.Exam.QuestionManagement.QuestionAnswers;
using SuperAbp.Exam.QuestionManagement.Questions;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using SuperAbp.Exam.QuestionManagement.QuestionRepos;
using SuperAbp.MenuManagement.EntityFrameworkCore;

namespace SuperAbp.Exam.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class ExamDbContext :
    AbpDbContext<ExamDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }

    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }

    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion Entities from the modules

    public DbSet<Question> Questions { get; set; }
    public DbSet<QuestionAnswer> QuestionAnswers { get; set; }
    public DbSet<QuestionRepo> QuestionRepos { get; set; }

    public ExamDbContext(DbContextOptions<ExamDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();
        builder.ConfigureMenuManagement();

        builder.Entity<Question>(b =>
        {
            b.ToTable(ExamConsts.DbTablePrefix + "Questions", ExamConsts.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureAuditedAggregateRoot();

            b.Property(p => p.QuestionType).IsRequired();
            b.Property(p => p.Content).IsRequired().HasMaxLength(QuestionConsts.MaxContentLength);
            b.Property(p => p.Analysis).HasMaxLength(QuestionConsts.MaxAnalysisLength);
        });

        builder.Entity<QuestionAnswer>(b =>
        {
            b.ToTable(ExamConsts.DbTablePrefix + "QuestionAnswers", ExamConsts.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureAuditedAggregateRoot();

            b.Property(p => p.Content).IsRequired().HasMaxLength(QuestionAnswerConsts.MaxContentLength);
            b.Property(p => p.Analysis).HasMaxLength(QuestionAnswerConsts.MaxAnalysisLength);
        });

        builder.Entity<QuestionRepo>(b =>
        {
            b.ToTable(ExamConsts.DbTablePrefix + "QuestionRepositories", ExamConsts.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureAuditedAggregateRoot();

            b.Property(p => p.Title).IsRequired().HasMaxLength(QuestionRepoConsts.MaxTitleLength);
            b.Property(p => p.Remark).HasMaxLength(QuestionRepoConsts.MaxRemarkLength);
        });
    }
}