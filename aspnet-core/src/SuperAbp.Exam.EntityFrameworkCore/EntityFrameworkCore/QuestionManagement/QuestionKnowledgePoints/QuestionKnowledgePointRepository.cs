using SuperAbp.Exam.QuestionManagement.QuestionKnowledgePoints;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using System.Threading;

namespace SuperAbp.Exam.EntityFrameworkCore.QuestionManagement.QuestionKnowledgePoints;

public class QuestionKnowledgePointRepository(IDbContextProvider<IExamDbContext> dbContextProvider)
    : EfCoreRepository<IExamDbContext, QuestionKnowledgePoint>(dbContextProvider), IQuestionKnowledgePointRepository
{
    public async Task<List<QuestionKnowledgePoint>> GetByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken = default)
    {
        return await GetListAsync(qk => qk.QuestionId == questionId, cancellationToken: cancellationToken);
    }
}