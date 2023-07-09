using System;
using System.Collections.Generic;
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
    /// <returns></returns>
    Task<int> GetCountAsync(Guid questionRepositoryId);

    /// <summary>
    /// 数量
    /// </summary>
    /// <param name="questionRepositoryId">题库Id</param>
    /// <param name="questionType">问题类型</param>
    /// <returns></returns>
    Task<int> GetCountAsync(Guid questionRepositoryId, QuestionType questionType);

    /// <summary>
    /// 列表
    /// </summary>
    /// <param name="questionRepositoryId">题库Id</param>
    /// <returns></returns>
    Task<List<Question>> GetListAsync(Guid questionRepositoryId);

    /// <summary>
    /// 是否存在
    /// </summary>
    /// <param name="questionRepositoryId"></param>
    /// <param name="questionId"></param>
    /// <returns></returns>
    Task<bool> AnyAsync(Guid questionRepositoryId, Guid questionId);
}