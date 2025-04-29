using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.QuestionManagement.QuestionKnowledgePoints;

public interface IQuestionKnowledgePointRepository : IRepository<QuestionKnowledgePoint>
{
    Task<List<QuestionKnowledgePoint>> GetByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken = default);
}