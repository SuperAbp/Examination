using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.QuestionManagement.QuestionKnowledgePoints;

public interface IQuestionKnowledgePointRepository : IRepository<QuestionKnowledgePoint>
{
    Task<List<QuestionKnowledgePoint>> GetByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="questionId">题目Id</param>
    /// <returns></returns>
    Task DeleteByQuestionIdAsync(Guid questionId);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="knowledgePointId">知识点Id</param>
    /// <returns></returns>
    Task DeleteByKnowledgePointIdAsync(Guid knowledgePointId);
}