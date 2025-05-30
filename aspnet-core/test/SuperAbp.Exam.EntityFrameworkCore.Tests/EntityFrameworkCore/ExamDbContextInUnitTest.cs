using Microsoft.EntityFrameworkCore;
using SmartEnum.EFCore;
using SuperAbp.Exam.ExamManagement.Exams;
using SuperAbp.Exam.ExamManagement.UserExamQuestionReviews;
using SuperAbp.Exam.ExamManagement.UserExamQuestions;
using SuperAbp.Exam.ExamManagement.UserExams;
using SuperAbp.Exam.Favorites;
using SuperAbp.Exam.KnowledgePoints;
using SuperAbp.Exam.PaperManagement.PaperQuestionRules;
using SuperAbp.Exam.PaperManagement.Papers;
using SuperAbp.Exam.QuestionManagement.QuestionAnswers;
using SuperAbp.Exam.QuestionManagement.QuestionBanks;
using SuperAbp.Exam.QuestionManagement.QuestionKnowledgePoints;
using SuperAbp.Exam.QuestionManagement.Questions;
using SuperAbp.Exam.TrainingManagement;
using SuperAbp.MenuManagement.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
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

namespace SuperAbp.Exam.EntityFrameworkCore;

[ReplaceDbContext(typeof(IExamDbContext))]
public class ExamDbContextInUnitTest : AbpDbContext<ExamDbContextInUnitTest>, IExamDbContext
{
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; }
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<KnowledgePoint> KnowledgePoints { get; set; }
    public DbSet<QuestionAnswer> QuestionAnswers { get; set; }
    public DbSet<QuestionBank> QuestionBanks { get; set; }
    public DbSet<Paper> Papers { get; set; }
    public DbSet<QuestionKnowledgePoint> QuestionKnowledgePoints { get; set; }
    public DbSet<PaperQuestionRule> PaperQuestionRules { get; set; }
    public DbSet<Examination> Exams { get; set; }
    public DbSet<UserExam> UserExams { get; set; }
    public DbSet<UserExamQuestion> UerExamQuestions { get; set; }
    public DbSet<UserExamQuestionReview> UserExamQuestionReviews { get; set; }
    public DbSet<Training> Trains { get; set; }
    public DbSet<Favorite> Favorites { get; set; }

    public ExamDbContextInUnitTest(DbContextOptions<ExamDbContextInUnitTest> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
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

        builder.Entity<KnowledgePoint>(b =>
        {
            b.ToTable(ExamConsts.DbTablePrefix + "KnowledgePoints", ExamConsts.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAudited();

            b.Property(p => p.Name).IsRequired().HasMaxLength(KnowledgePointConsts.MaxNameLength);
        });

        builder.Entity<QuestionAnswer>(b =>
        {
            b.ToTable(ExamConsts.DbTablePrefix + "QuestionAnswers", ExamConsts.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureAuditedAggregateRoot();

            b.Property(p => p.Content).IsRequired().HasMaxLength(QuestionAnswerConsts.MaxContentLength);
            b.Property(p => p.Analysis).HasMaxLength(QuestionAnswerConsts.MaxAnalysisLength);
            b.Property(p => p.Sort).IsRequired().HasDefaultValue(0);
        });

        builder.Entity<QuestionBank>(b =>
        {
            b.ToTable(ExamConsts.DbTablePrefix + "QuestionBanks", ExamConsts.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureAuditedAggregateRoot();

            b.Property(p => p.Title).IsRequired().HasMaxLength(QuestionBankConsts.MaxTitleLength);
            b.Property(p => p.Remark).HasMaxLength(QuestionBankConsts.MaxRemarkLength);
        });

        builder.Entity<QuestionKnowledgePoint>(b =>
        {
            b.ToTable(ExamConsts.DbTablePrefix + "QuestionKnowledgePoints", ExamConsts.DbSchema);
            b.ConfigureByConvention();

            b.HasKey(qk => new { qk.QuestionId, qk.KnowledgePointId });
            b.HasIndex(qk => new { qk.QuestionId, qk.KnowledgePointId });
        });

        builder.Entity<Paper>(b =>
        {
            b.ToTable(ExamConsts.DbTablePrefix + "Papers", ExamConsts.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureAuditedAggregateRoot();

            b.Property(p => p.Name).IsRequired().HasMaxLength(PaperConsts.MaxNameLength);
            b.Property(p => p.Description).HasMaxLength(PaperConsts.MaxDescriptionLength);
        });

        builder.Entity<PaperQuestionRule>(b =>
        {
            b.ToTable(ExamConsts.DbTablePrefix + "PaperQuestionRules", ExamConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<Examination>(b =>
        {
            b.ToTable(ExamConsts.DbTablePrefix + "Examination", ExamConsts.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureAuditedAggregateRoot();

            b.Property(p => p.Name).IsRequired().HasMaxLength(PaperConsts.MaxNameLength);
            b.Property(p => p.Description).HasMaxLength(PaperConsts.MaxDescriptionLength);

            b.HasIndex(p => p.PaperId);
        });

        builder.Entity<UserExam>(b =>
        {
            b.ToTable(ExamConsts.DbTablePrefix + "UserExam", ExamConsts.DbSchema);
            b.Property(e => e.TotalScore)
                .HasConversion<double>();
            b.ConfigureByConvention();
            b.ConfigureAuditedAggregateRoot();
        });

        builder.Entity<UserExamQuestion>(b =>
        {
            b.ToTable(ExamConsts.DbTablePrefix + "UserExamQuestion", ExamConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(p => p.Answers).HasMaxLength(UserExamQuestionConsts.MaxAnswersLength);
            b.Property(p => p.Reason).HasMaxLength(UserExamQuestionConsts.MaxReasonLength);
        });

        builder.Entity<UserExamQuestionReview>(b =>
        {
            b.ToTable(ExamConsts.DbTablePrefix + "UserExamQuestionReview", ExamConsts.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAudited();

            b.Property(p => p.Reason).HasMaxLength(UserExamQuestionReviewConsts.MaxReasonLength);
        });

        builder.Entity<Training>(b =>
        {
            b.ToTable(ExamConsts.DbTablePrefix + "Training", ExamConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<Favorite>(b =>
        {
            b.ToTable(ExamConsts.DbTablePrefix + "Favorites", ExamConsts.DbSchema);
            b.ConfigureByConvention();
        });
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.ConfigureSmartEnum();
    }
}