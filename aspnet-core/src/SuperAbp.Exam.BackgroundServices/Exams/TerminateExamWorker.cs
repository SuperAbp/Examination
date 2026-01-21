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

public class TerminateExamWorker : AsyncPeriodicBackgroundWorkerBase
{
    public TerminateExamWorker(AbpAsyncTimer timer, IServiceScopeFactory serviceScopeFactory) : base(timer, serviceScopeFactory)
    {
        Timer.Period = 60 * 60 * 1000;
#if DEBUG
        timer.RunOnStart = true;
#else
        timer.RunOnStart = true;
#endif
    }

    [UnitOfWork]
    protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        Logger.LogInformation("Started examination terminate...");

        await TerminateAsync(workerContext);

        Logger.LogInformation($"Successfully completed host examination terminate.");

        ITenantRepository tenantRepository = workerContext.ServiceProvider.GetRequiredService<ITenantRepository>();
        ICurrentTenant currentTenant = workerContext.ServiceProvider.GetRequiredService<ICurrentTenant>();
        var tenants = await tenantRepository.GetListAsync();
        foreach (var tenant in tenants)
        {
            using (currentTenant.Change(tenant.Id))
            {
                await TerminateAsync(workerContext);
            }

            Logger.LogInformation($"Successfully completed {tenant.Name} tenant examination terminate.");
        }
        Logger.LogInformation("Successfully completed all examination terminate.");
    }

    private static async Task TerminateAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        IExamRepository examRepository = workerContext.ServiceProvider.GetRequiredService<IExamRepository>();
        IUserExamRepository userExamRepository = workerContext.ServiceProvider.GetRequiredService<IUserExamRepository>();
        IClock clock = workerContext.ServiceProvider.GetRequiredService<IClock>();

        List<Examination> examinations = await (await examRepository.GetQueryableAsync())
                    .Where(e => e.EndTime < clock.Now && e.Status == ExaminationStatus.Published)
                    .ToListAsync();
        if (examinations.Count == 0)
        {
            return;
        }
        List<UserExam> userExams = [];
        foreach (var item in examinations)
        {
            item.Terminate(clock.Now);

            userExams.AddRange(await (await userExamRepository.GetQueryableAsync())
                .Where(c => c.ExamId == item.Id && c.IsActive &&
                    (c.Status == UserExamStatus.Waiting || c.Status == UserExamStatus.InProgress))
                .ToListAsync());
        }
        await examRepository.UpdateManyAsync(examinations);

        if (userExams.Count == 0)
        {
            return;
        }

        foreach (UserExam userExam in userExams)
        {
            userExam.Timeout();
            userExam.FinishedTime = clock.Now;
        }
        await userExamRepository.UpdateManyAsync(userExams);
    }
}