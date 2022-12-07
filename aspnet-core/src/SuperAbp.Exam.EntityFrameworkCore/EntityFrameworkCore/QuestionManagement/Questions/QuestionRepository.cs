using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using SuperAbp.Exam.QuestionManagement.Questions;
using System;

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

    // TODO:编写仓储代码
}