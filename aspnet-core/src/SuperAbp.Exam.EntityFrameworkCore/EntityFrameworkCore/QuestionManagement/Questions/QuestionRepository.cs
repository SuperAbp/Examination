using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using SuperAbp.Exam.QuestionManagement.Questions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SuperAbp.Exam.EntityFrameworkCore.QuestionManagement.Questions;

/// <summary>
/// 问题
/// </summary>
public class QuestionRepository : EfCoreRepository<ExamDbContext, Question, Guid>, IQuestionRepository
{
    /// <summary>
    /// .ctor
    ///</summary>
    public QuestionRepository(
        IDbContextProvider<ExamDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<int> GetCountAsync(Guid questionRepositoryId)
    {
        return await (await GetDbSetAsync())
            .Where(r => r.QuestionRepositoryId == questionRepositoryId)
            .CountAsync();
    }

    public async Task<int> GetCountAsync(Guid questionRepositoryId, QuestionType questionType)
    {
        return await (await GetDbSetAsync())
            .Where(r => r.QuestionRepositoryId == questionRepositoryId && r.QuestionType == questionType)
            .CountAsync();
    }

    public async Task<List<QuestionType>> GetQuestionTypesAsync(Guid questionRepositoryId)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.Where(q => q.QuestionRepositoryId == questionRepositoryId)
            .GroupBy(q => q.QuestionType)
            .Select(q => q.Key)
            .ToListAsync();
    }

    public async Task<List<Question>> GetListAsync(Guid questionRepositoryId)
    {
        return await GetListAsync(q => q.QuestionRepositoryId == questionRepositoryId);
    }

    public async Task<bool> AnyAsync(Guid questionRepositoryId, Guid questionId)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.AnyAsync(q => q.QuestionRepositoryId == questionRepositoryId && q.Id == questionId);
    }
}