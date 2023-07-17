using SuperAbp.Exam.QuestionManagement.Questions;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SuperAbp.Exam.TrainingManagement;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SuperAbp.Exam.EntityFrameworkCore.TrainingManagement;

public class TrainingRepository : EfCoreRepository<ExamDbContext, Training, Guid>, ITrainingRepository
{
    public TrainingRepository(IDbContextProvider<ExamDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<bool> AnyQuestionAsync(Guid questionId)
    {
        var dbContext = await GetDbSetAsync();
        return await dbContext.AnyAsync(t => t.QuestionId == questionId);
    }
}