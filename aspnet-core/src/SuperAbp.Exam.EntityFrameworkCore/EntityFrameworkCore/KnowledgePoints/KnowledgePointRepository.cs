using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using SuperAbp.Exam.KnowledgePoints;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SuperAbp.Exam.QuestionManagement.QuestionKnowledgePoints;

namespace SuperAbp.Exam.EntityFrameworkCore.KnowledgePoints;

public class KnowledgePointRepository(IDbContextProvider<IExamDbContext> dbContextProvider)
    : EfCoreRepository<IExamDbContext, KnowledgePoint, Guid>(dbContextProvider), IKnowledgePointRepository
{
    public async Task<List<KnowledgePoint>> GetByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();
        var questionBankQueryable = dbContext.Set<QuestionKnowledgePoint>().AsQueryable();

        return await (from kp in (await GetQueryableAsync())
                      join qkp in questionBankQueryable on kp.Id equals qkp.KnowledgePointId
                      where qkp.QuestionId == questionId
                      select kp)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<KnowledgePoint>> GetListAsync(string? name = null, CancellationToken cancellationToken = default)
    {
        var queryable = await GetQueryableAsync();
        return await queryable
            .WhereIf(!name.IsNullOrWhiteSpace(), x => x.Name.Contains(name))
            .ToListAsync(cancellationToken);
    }
}