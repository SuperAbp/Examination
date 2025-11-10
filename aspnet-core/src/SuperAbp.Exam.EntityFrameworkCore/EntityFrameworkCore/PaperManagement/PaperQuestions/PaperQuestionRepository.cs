using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SuperAbp.Exam.PaperManagement.PaperQuestions;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SuperAbp.Exam.EntityFrameworkCore.PaperManagement.PaperQuestions;

public class PaperQuestionRepository(IDbContextProvider<IExamDbContext> dbContextProvider)
    : EfCoreRepository<IExamDbContext, PaperQuestion, Guid>(dbContextProvider), IPaperQuestionRepository
{
    public async Task<List<PaperQuestion>> GetListByPaperSectionIdAsync(Guid paperSectionId, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(p => p.PaperSectionId == paperSectionId)
            .OrderBy(p => p.Order)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<PaperQuestion>> GetListByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .Where(p => p.QuestionId == questionId)
            .ToListAsync(cancellationToken);
    }

    public async Task<PaperQuestion> GetByPaperSectionIdAndQuestionIdAsync(Guid paperSectionId, Guid questionId, CancellationToken cancellationToken = default)
    {
        return await (await GetQueryableAsync())
            .FirstOrDefaultAsync(p => p.PaperSectionId == paperSectionId && p.QuestionId == questionId, cancellationToken);
    }
}
