using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using SuperAbp.Exam.ExamManagement.UserExamQuestions;

namespace SuperAbp.Exam.EntityFrameworkCore.ExamManagement.UserExamQuestions
{
    public class UserExamQuestionRepository(IDbContextProvider<IExamDbContext> dbContextProvider)
        : EfCoreRepository<IExamDbContext, UserExamQuestion, Guid>(dbContextProvider), IUserExamQuestionRepository
    {
    }
}