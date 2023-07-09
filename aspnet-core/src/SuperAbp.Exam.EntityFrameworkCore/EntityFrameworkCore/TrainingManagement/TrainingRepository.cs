using SuperAbp.Exam.QuestionManagement.Questions;
using System;
using SuperAbp.Exam.TrainingManagement;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SuperAbp.Exam.EntityFrameworkCore.TrainingManagement;

public class TrainingRepository : EfCoreRepository<ExamDbContext, Training, Guid>, ITrainingRepository
{
    public TrainingRepository(IDbContextProvider<ExamDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}