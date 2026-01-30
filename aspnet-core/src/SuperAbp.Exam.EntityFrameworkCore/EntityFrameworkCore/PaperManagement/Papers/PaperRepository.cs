using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SuperAbp.Exam.PaperManagement.Papers;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SuperAbp.Exam.EntityFrameworkCore.PaperManagement.Papers;

/// <summary>
/// 试卷
/// </summary>
public class PaperRepository(IDbContextProvider<IExamDbContext> dbContextProvider)
    : EfCoreRepository<IExamDbContext, Paper, Guid>(dbContextProvider), IPaperRepository
{
    public async Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync()).AnyAsync(e => e.Name == name, cancellationToken);
    }

    public async Task<List<Paper>> GetPapersByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var papers = await dbContext.Papers
            .Where(p => p.PaperSections
                .Any(s => s.PaperQuestions
                    .Any(pq => pq.QuestionId == questionId)))
            .Include(p => p.PaperSections)
            .ThenInclude(s => s.PaperQuestions)
            .ToListAsync(cancellationToken);

        return papers;
    }

    public async Task<List<Paper>> GetPapersByKnowledgePointIdAsync(Guid knowledgePointId, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbContextAsync();

        var papers = await dbContext.Papers
            .Where(p => p.PaperSections
                .Any(s => s.PaperQuestionRules
                    .Any(r => r.KnowledgePointId == knowledgePointId)))
            .Include(p => p.PaperSections)
            .ThenInclude(s => s.PaperQuestionRules)
            .ToListAsync(cancellationToken);

        return papers;
    }
}