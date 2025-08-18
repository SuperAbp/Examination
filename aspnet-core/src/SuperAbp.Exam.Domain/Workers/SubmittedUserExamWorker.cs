using Microsoft.Extensions.DependencyInjection;
using SuperAbp.Exam.ExamManagement.UserExams;
using SuperAbp.Exam.Settings;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SuperAbp.Exam.ExamManagement.Exams;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Settings;
using Volo.Abp.Threading;
using System;
using System.Collections.Generic;
using Volo.Abp.Timing;

namespace SuperAbp.Exam.Workers;

public class SubmittedUserExamWorker : AsyncPeriodicBackgroundWorkerBase
{
    public SubmittedUserExamWorker(AbpAsyncTimer timer, IServiceScopeFactory serviceScopeFactory) : base(timer, serviceScopeFactory)
    {
        Timer.Period = 60 * 60 * 1000;
        Timer.RunOnStart = true;
    }

    protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        var userExamRepository = workerContext.ServiceProvider.GetRequiredService<IUserExamRepository>();
        IClock clock = workerContext.ServiceProvider.GetRequiredService<IClock>();
        DateTime now = clock.Now.AddMinutes(10);
        List<UserExam> timeoutUserExams = await userExamRepository.GetTimeoutUserExamsAsync(now);
        foreach (UserExam userExam in timeoutUserExams)
        {
            userExam.Status = UserExamStatus.Timeout;
            userExam.FinishedTime = now;
        }
        await userExamRepository.UpdateManyAsync(timeoutUserExams);
    }
}