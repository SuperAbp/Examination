using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using SuperAbp.Exam.KnowledgePoints;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SuperAbp.Exam.EntityFrameworkCore.KnowledgePoints;

public class KnowledgePointRepository(IDbContextProvider<ExamDbContext> dbContextProvider)
    : EfCoreRepository<ExamDbContext, KnowledgePoint, Guid>(dbContextProvider), IKnowledgePointRepository
{
    public async Task<List<KnowledgePoint>> GetListAsync(string? name = null, CancellationToken cancellationToken = default)
    {
        var queryable = await GetQueryableAsync();
        return await queryable
            .WhereIf(!name.IsNullOrWhiteSpace(), x => x.Name.Contains(name))
            .ToListAsync(cancellationToken);
    }
}