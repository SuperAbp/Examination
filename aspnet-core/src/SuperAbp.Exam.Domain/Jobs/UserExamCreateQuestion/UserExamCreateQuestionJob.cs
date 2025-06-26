using SuperAbp.Exam.ExamManagement.UserExams;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.DependencyInjection;

namespace SuperAbp.Exam.Jobs.UserExamCreateQuestion;

public class UserExamCreateQuestionJob(IUserExamRepository userExamRepository, UserExamManager userExamManager) : AsyncBackgroundJob<UserExamCreateQuestionArgs>, ITransientDependency
{
    public override async Task ExecuteAsync(UserExamCreateQuestionArgs args)
    {
        await userExamManager.CreateQuestionsAsync(args.UserExamId);
    }
}