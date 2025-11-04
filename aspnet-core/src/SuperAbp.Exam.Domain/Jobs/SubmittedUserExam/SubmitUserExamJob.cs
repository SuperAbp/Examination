using SuperAbp.Exam.ExamManagement.UserExams;
using SuperAbp.Exam.Jobs.UserExamCreateQuestion;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace SuperAbp.Exam.Jobs.SubmittedUserExam;

public class SubmitUserExamJob(IUnitOfWorkManager unitOfWorkManager, UserExamManager userExamManager, ICurrentTenant currentTenant)
    : AsyncBackgroundJob<SubmitUserExamArgs>, ITransientDependency
{
    public override async Task ExecuteAsync(SubmitUserExamArgs args)
    {
        using (currentTenant.Change(args.TenantId))
        {
            await userExamManager.SubmitUserExamAsync(args.ExamId);
        }
    }
}