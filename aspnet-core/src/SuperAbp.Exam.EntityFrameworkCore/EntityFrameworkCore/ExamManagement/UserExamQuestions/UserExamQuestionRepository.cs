using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using SuperAbp.Exam.ExamManagement.UserExamQuestions;
using SuperAbp.Exam.ExamManagement.UserExams;
using SuperAbp.Exam.QuestionManagement.Questions;
using SuperAbp.Exam.QuestionManagement.Questions.QuestionAnswers;

namespace SuperAbp.Exam.EntityFrameworkCore.ExamManagement.UserExamQuestions
{
    public class UserExamQuestionRepository(IDbContextProvider<IExamDbContext> dbContextProvider)
        : EfCoreRepository<IExamDbContext, UserExamQuestion, Guid>(dbContextProvider), IUserExamQuestionRepository
    {
    }
}