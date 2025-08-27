using Microsoft.EntityFrameworkCore;
using SuperAbp.Exam.ExamManagement.Exams;
using SuperAbp.Exam.ExamManagement.UserExamQuestionReviews;
using SuperAbp.Exam.ExamManagement.UserExamQuestions;
using SuperAbp.Exam.ExamManagement.UserExams;
using SuperAbp.Exam.Favorites;
using SuperAbp.Exam.KnowledgePoints;
using SuperAbp.Exam.MistakesReviews;
using SuperAbp.Exam.PaperManagement.PaperQuestionRules;
using SuperAbp.Exam.PaperManagement.Papers;
using SuperAbp.Exam.QuestionManagement.QuestionBanks;
using SuperAbp.Exam.QuestionManagement.QuestionKnowledgePoints;
using SuperAbp.Exam.QuestionManagement.Questions;
using SuperAbp.Exam.QuestionManagement.Questions.QuestionAnswers;
using SuperAbp.Exam.TrainingManagement;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.TenantManagement;

namespace SuperAbp.Exam.EntityFrameworkCore;

public interface IExamDbContext : IEfCoreDbContext
{
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
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; }

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }

    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion Entities from the modules

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
    public DbSet<MistakesReview> MistakesReviews { get; set; }
}