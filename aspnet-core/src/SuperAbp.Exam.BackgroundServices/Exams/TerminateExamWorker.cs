using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SuperAbp.Exam.ExamManagement.Exams;
using SuperAbp.Exam.ExamManagement.UserExams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace SuperAbp.Exam.BackgroundServices.Exams;

public class TerminateExamWorker : AsyncPeriodicBackgroundWorkerBase
{
    public TerminateExamWorker(AbpAsyncTimer timer, IServiceScopeFactory serviceScopeFactory) : base(timer, serviceScopeFactory)
    {
        Timer.Period = 60 * 60 * 1000;
#if DEBUG
        //timer.RunOnStart = true;
#else
        timer.RunOnStart = true;
#endif
    }

    [UnitOfWork]
    protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        IExamRepository examRepository = workerContext.ServiceProvider.GetRequiredService<IExamRepository>();
        IUserExamRepository userExamRepository = workerContext.ServiceProvider.GetRequiredService<IUserExamRepository>();
        IClock clock = workerContext.ServiceProvider.GetRequiredService<IClock>();
        List<Examination> examinations = await (await examRepository.GetQueryableAsync())
            .Where(e => e.EndTime < clock.Now && e.Status == ExaminationStatus.Published)
            .ToListAsync();
        List<UserExam> userExams = [];
        foreach (var item in examinations)
        {
            item.Terminate(clock.Now);

            userExams.AddRange(await (await userExamRepository.GetQueryableAsync())
                .Where(c => c.ExamId == item.Id && c.IsActive)
                .ToListAsync());
        }
        foreach (UserExam userExam in userExams)
        {
            userExam.Timeout();
            userExam.FinishedTime = clock.Now;
        }

        await examRepository.UpdateManyAsync(examinations);
        await userExamRepository.UpdateManyAsync(userExams);
    }
}