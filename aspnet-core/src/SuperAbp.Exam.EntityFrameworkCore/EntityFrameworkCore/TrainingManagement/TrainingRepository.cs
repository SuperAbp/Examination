using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SuperAbp.Exam.TrainingManagement;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using System.Threading;

namespace SuperAbp.Exam.EntityFrameworkCore.TrainingManagement;

public class TrainingRepository(IDbContextProvider<ExamDbContext> dbContextProvider) : EfCoreRepository<ExamDbContext, Training, Guid>(dbContextProvider), ITrainingRepository
{
    public async Task<bool> AnyQuestionAsync(Guid questionId, CancellationToken cancellationToken = default)
    {
        var dbContext = await GetDbSetAsync();
        return await dbContext.AnyAsync(t => t.QuestionId == questionId, cancellationToken);
    }
}