using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SuperAbp.Exam.Announcements;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Threading;
using Volo.Abp.Timing;
using Volo.Abp.TenantManagement;
using Volo.Abp.Uow;

namespace SuperAbp.Exam.BackgroundServices.Announcements;

/// <summary>
/// 公告自动发布和过期处理后台任务
/// </summary>
public class AnnouncementAutoPublishWorker : AsyncPeriodicBackgroundWorkerBase
{
    public AnnouncementAutoPublishWorker(AbpAsyncTimer timer, IServiceScopeFactory serviceScopeFactory)
        : base(timer, serviceScopeFactory)
    {
        Timer.Period = 60000;
        timer.RunOnStart = true;
    }

    [UnitOfWork(isTransactional: false)]
    protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        var logger = workerContext.ServiceProvider.GetRequiredService<ILogger<AnnouncementAutoPublishWorker>>();
        var clock = workerContext.ServiceProvider.GetRequiredService<IClock>();

        logger.LogInformation("Started announcement auto publish...");

        await ProcessAnnouncementsAsync(workerContext);
        logger.LogInformation("Successfully completed host announcement auto publish.");

        ITenantRepository tenantRepository = workerContext.ServiceProvider.GetRequiredService<ITenantRepository>();
        ICurrentTenant currentTenant = workerContext.ServiceProvider.GetRequiredService<ICurrentTenant>();
        var tenants = await tenantRepository.GetListAsync();
        foreach (var tenant in tenants)
        {
            using (currentTenant.Change(tenant.Id))
            {
                await ProcessAnnouncementsAsync(workerContext);
            }

            logger.LogInformation($"Successfully completed {tenant.Name} tenant announcement auto publish.");
        }

        logger.LogInformation("Successfully completed all announcement auto publish.");
    }

    private async Task ProcessAnnouncementsAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        var logger = workerContext.ServiceProvider.GetRequiredService<ILogger<AnnouncementAutoPublishWorker>>();
        var repository = workerContext.ServiceProvider.GetRequiredService<IAnnouncementRepository>();
        var clock = workerContext.ServiceProvider.GetRequiredService<IClock>();

        logger.LogInformation("AnnouncementAutoPublishWorker started at {Time}", clock.Now);

        try
        {
            await PublishScheduledAnnouncementsAsync(repository, clock, logger);

            await ExpireAnnouncementsAsync(repository, clock, logger);

            logger.LogInformation("AnnouncementAutoPublishWorker completed at {Time}", clock.Now);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in AnnouncementAutoPublishWorker at {Time}", clock.Now);
        }
    }

    /// <summary>
    /// 发布定时公告
    /// </summary>
    private async Task PublishScheduledAnnouncementsAsync(
        IAnnouncementRepository repository,
        IClock clock,
        ILogger logger)
    {
        var queryable = await repository.GetQueryableAsync();
        var now = clock.Now;

        var toPublish = await queryable
            .Where(a => !a.IsPublished && a.ScheduledPublishTime.HasValue && a.ScheduledPublishTime.Value <= now)
            .ToListAsync();

        if (toPublish.Any())
        {
            logger.LogInformation("Found {Count} announcements to publish", toPublish.Count);

            foreach (var announcement in toPublish)
            {
                announcement.Publish();
                await repository.UpdateAsync(announcement);
                logger.LogInformation("Published announcement {Id}: {Title}", announcement.Id, announcement.Title);
            }
        }
    }

    /// <summary>
    /// 处理过期公告
    /// </summary>
    private async Task ExpireAnnouncementsAsync(
        IAnnouncementRepository repository,
        IClock clock,
        ILogger logger)
    {
        var queryable = await repository.GetQueryableAsync();
        var now = clock.Now;

        var toExpire = await queryable
            .Where(a => a.IsPublished && a.ScheduledExpirationTime.HasValue && a.ScheduledExpirationTime.Value <= now)
            .ToListAsync();

        if (toExpire.Any())
        {
            logger.LogInformation("Found {Count} announcements to expire", toExpire.Count);

            foreach (var announcement in toExpire)
            {
                announcement.Unpublish();
                await repository.UpdateAsync(announcement);
                logger.LogInformation("Expired announcement {Id}: {Title}", announcement.Id, announcement.Title);
            }
        }
    }
}