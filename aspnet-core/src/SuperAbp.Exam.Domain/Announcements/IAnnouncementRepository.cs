using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.Announcements;

public interface IAnnouncementRepository : IRepository<Announcement, Guid>
{
    /// <summary>
    /// 获取有效的公告列表（Include 分类）
    /// </summary>
    Task<List<Announcement>> GetEffectiveListAsync(DateTime now, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取分类下的有效公告列表
    /// </summary>
    Task<List<Announcement>> GetEffectiveListByCategoryIdAsync(Guid categoryId, DateTime now, CancellationToken cancellationToken = default);

    /// <summary>
    /// 数量
    /// </summary>
    Task<int> GetCountAsync(
        string? title = null,
        Guid? categoryId = null,
        bool? isPublished = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 列表
    /// </summary>
    Task<List<Announcement>> GetListAsync(
        string? sorting = null,
        int skipCount = 0,
        int maxResultCount = int.MaxValue,
        string? title = null,
        Guid? categoryId = null,
        bool? isPublished = null,
        CancellationToken cancellationToken = default);
}