using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.QuestionManagement.Questions;

/// <summary>
/// 问题
/// </summary>
public interface IQuestionRepository : IRepository<Question, Guid>
{
    /// <summary>
    /// 数量
    /// </summary>
    /// <param name="questionRepositoryId">题库Id</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> GetCountAsync(Guid questionRepositoryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 数量
    /// </summary>
    /// <param name="questionRepositoryId">题库Id</param>
    /// <param name="questionType">问题类型</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> GetCountAsync(Guid questionRepositoryId, QuestionType questionType, CancellationToken cancellationToken = default);

    /// <summary>
    /// 题型
    /// </summary>
    /// <param name="questionRepositoryId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<QuestionType>> GetQuestionTypesAsync(Guid questionRepositoryId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 列表
    /// </summary>
    /// <param name="sorting"></param>
    /// <param name="skipCount"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="questionRepositoryId">题库Id</param>
    /// <param name="questionType">题型</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<Question>> GetListAsync(
        string? sorting = null,
        int skipCount = 0,
        int maxResultCount = int.MaxValue,
        Guid? questionRepositoryId = null,
        QuestionType? questionType = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 随机列表
    /// </summary>
    /// <param name="maxResultCount"></param>
    /// <param name="questionRepositoryId">题库Id</param>
    /// <param name="questionType">题型</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<Question>> GetRandomListAsync(
        int maxResultCount = int.MaxValue,
        Guid? questionRepositoryId = null,
        QuestionType? questionType = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 是否存在
    /// </summary>
    /// <param name="questionRepositoryId"></param>
    /// <param name="questionId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> AnyAsync(Guid questionRepositoryId, Guid questionId, CancellationToken cancellationToken = default);

    Task<bool> ContentExistsAsync(string content, CancellationToken cancellationToken = default);
}