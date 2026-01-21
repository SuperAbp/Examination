using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SuperAbp.Exam.ExamManagement.Exams;
using SuperAbp.Exam.ExamManagement.UserExams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TenantManagement;
using Volo.Abp.Threading;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace SuperAbp.Exam.BackgroundServices.Exams;

public class SubmittedUserExamWorker : AsyncPeriodicBackgroundWorkerBase
{
    public SubmittedUserExamWorker(AbpAsyncTimer timer, IServiceScopeFactory serviceScopeFactory) : base(timer, serviceScopeFactory)
    {
        Timer.Period = 60 * 60 * 1000;
#if DEBUG
        timer.RunOnStart = true;
#else
        timer.RunOnStart = true;
#endif
    }

    [UnitOfWork(isTransactional: false)]
    protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        Logger.LogInformation("Started user examination submitted...");
        await SubmittedAsync(workerContext);
        Logger.LogInformation($"Successfully completed host user examination submitted.");

        ITenantRepository tenantRepository = workerContext.ServiceProvider.GetRequiredService<ITenantRepository>();
        ICurrentTenant currentTenant = workerContext.ServiceProvider.GetRequiredService<ICurrentTenant>();
        var tenants = await tenantRepository.GetListAsync();
        foreach (var tenant in tenants)
        {
            using (currentTenant.Change(tenant.Id))
            {
                await SubmittedAsync(workerContext);
            }

            Logger.LogInformation($"Successfully completed {tenant.Name} tenant user examination submitted.");
        }
        Logger.LogInformation("Successfully completed all user examination submitted.");
    }

    private static async Task SubmittedAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        var userExamRepository = workerContext.ServiceProvider.GetRequiredService<IUserExamRepository>();
        var examRepository = workerContext.ServiceProvider.GetRequiredService<IExamRepository>();
        IClock clock = workerContext.ServiceProvider.GetRequiredService<IClock>();

        var userExamQueryable = await userExamRepository.GetQueryableAsync();
        var examQueryable = await examRepository.GetQueryableAsync();

        DateTime endTime = clock.Now.AddMinutes(10);
        List<UserExam> timeoutUserExams = await (from ue in userExamQueryable
                                                 join e in examQueryable on ue.ExamId equals e.Id
                                                 where
                                                     (ue.Status == UserExamStatus.Waiting || ue.Status == UserExamStatus.InProgress) &&
                                                     (
                                                         (e.EndTime.HasValue && e.EndTime.Value < endTime)
                                                         || (ue.StartTime.HasValue && !ue.FinishedTime.HasValue && ue.StartTime.Value.AddMinutes(e.TotalTime) < endTime)
                                                         || (!ue.StartTime.HasValue && ue.CreationTime.AddMinutes(e.TotalTime) < endTime)
                                                     )
                                                 select ue)
                    .ToListAsync();
        foreach (UserExam userExam in timeoutUserExams)
        {
            userExam.Timeout();
            userExam.FinishedTime = clock.Now;
        }
        await userExamRepository.UpdateManyAsync(timeoutUserExams);
    }
}