using SuperAbp.Exam.ExamManagement.UserExams;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace SuperAbp.Exam.Jobs.UserExamCreateQuestion;

public class UserExamCreateQuestionJob(IUnitOfWorkManager unitOfWorkManager, UserExamManager userExamManager, ICurrentTenant currentTenant) : AsyncBackgroundJob<UserExamCreateQuestionArgs>, ITransientDependency
{
    [UnitOfWork]
    public override async Task ExecuteAsync(UserExamCreateQuestionArgs args)
    {
        using (currentTenant.Change(args.TenantId))
        {
            await userExamManager.CreateQuestionsAsync(args.UserExamId);
        }
    }
}