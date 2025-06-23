using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SuperAbp.Exam.TrainingManagement;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace SuperAbp.Exam.EntityFrameworkCore.TrainingManagement;

public class TrainingRepository(IDbContextProvider<IExamDbContext> dbContextProvider) : EfCoreRepository<IExamDbContext, Training, Guid>(dbContextProvider), ITrainingRepository
{
    public async Task<bool> AnyQuestionAsync(TrainingSource trainingSource, Guid questionId, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbSetAsync();
        return await dbContext.AnyAsync(t => t.TrainingSource == trainingSource && t.QuestionId == questionId, cancellationToken);
    }

    public async Task<List<Training>> GetListAsync(string? sorting = null,
        int skipCount = 0,
        int maxResultCount = int.MaxValue,
        int? trainingSource = null,
        Guid? questionRepositoryId = null,
        Guid? userId = null,
        CancellationToken cancellationToken = default)
    {
        var queryable = await GetQueryableAsync();

        return await queryable
            .WhereIf(trainingSource.HasValue, p => p.TrainingSource == trainingSource.Value)
            .WhereIf(questionRepositoryId.HasValue, p => p.QuestionBankId == questionRepositoryId.Value)
            .WhereIf(userId.HasValue, p => p.UserId == userId.Value)
            .OrderBy(string.IsNullOrWhiteSpace(sorting) ? TrainingConsts.DefaultSorting : sorting)
            .OrderBy(q => EF.Functions.Random())
            .PageBy(skipCount, maxResultCount)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}