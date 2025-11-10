using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SuperAbp.Exam.PaperManagement.PaperQuestionRules;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SuperAbp.Exam.EntityFrameworkCore.PaperManagement.PaperQuestionRules;

/// <summary>
/// 考试题库
/// </summary>
public class PaperQuestionRuleRepository(IDbContextProvider<IExamDbContext> dbContextProvider)
    : EfCoreRepository<IExamDbContext, PaperQuestionRule, Guid>(dbContextProvider), IPaperQuestionRuleRepository
{
    public async Task<PaperQuestionRule> GetAsync(Guid paperSectionId, Guid questionBankId, CancellationToken cancellationToken = default)
    {
        return await GetAsync(er => er.PaperSectionId == paperSectionId
                              && er.QuestionBankId == questionBankId, cancellationToken: cancellationToken);
    }

    public async Task<PaperQuestionRule?> FindAsync(Guid paperSectionId, Guid questionBankId, CancellationToken cancellationToken = default)
    {
        return await FindAsync(er => er.PaperSectionId == paperSectionId
                                     && er.QuestionBankId == questionBankId, cancellationToken: cancellationToken);
    }

    public async Task<List<PaperQuestionRule>> GetListAsync(
        string? sorting = null,
        int skipCount = 0,
        int maxResultCount = int.MaxValue,
        Guid? paperSectionId = null,
        CancellationToken cancellationToken = default)
    {
        var queryable = await GetQueryableAsync();

        return await queryable
             .WhereIf(paperSectionId.HasValue, p => p.PaperSectionId == paperSectionId.Value)
             .OrderBy(string.IsNullOrWhiteSpace(sorting) ? PaperQuestionRuleConsts.DefaultSorting : sorting)
             .PageBy(skipCount, maxResultCount)
             .ToListAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid paperSectionId, Guid questionBankId, CancellationToken cancellationToken = default)
    {
        await DeleteAsync(er => er.PaperSectionId == paperSectionId && er.QuestionBankId == questionBankId, cancellationToken: cancellationToken);
    }

    public async Task DeleteByPaperSectionIdAsync(Guid paperSectionId, CancellationToken cancellationToken = default)
    {
        await DeleteAsync(er => er.PaperSectionId == paperSectionId, cancellationToken: cancellationToken);
    }

    public async Task DeleteByPaperIdAsync(Guid paperId, CancellationToken cancellationToken = default)
    {
        await DeleteAsync(er => er.PaperSection.PaperId == paperId, cancellationToken: cancellationToken);
    }
}