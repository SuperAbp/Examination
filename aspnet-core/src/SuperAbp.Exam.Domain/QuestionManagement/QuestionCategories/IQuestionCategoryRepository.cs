using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.QuestionManagement.QuestionCategories;

/// <summary>
/// 问题类别
/// </summary>
public interface IQuestionCategoryRepository : IRepository<QuestionCategory, Guid>
{
    /// <summary>
    /// 数量
    /// </summary>
    /// <param name="name">名称</param>
    /// <param name="parentId">父级Id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> GetCountAsync(
        string? name = null,
        Guid? parentId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 列表
    /// </summary>
    /// <param name="name">名称</param>
    /// <param name="parentId">父级Id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<QuestionCategoryWithDetails>> GetListAsync(
        string? sorting = null,
        int skipCount = 0,
        int maxResultCount = int.MaxValue,
        string? name = null,
        Guid? parentId = null,
        CancellationToken cancellationToken = default);
}