using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using SuperAbp.Exam.ExamManagement.UserExamQuestions;
using SuperAbp.Exam.QuestionManagement.QuestionAnswers;
using SuperAbp.Exam.QuestionManagement.Questions;

namespace SuperAbp.Exam.EntityFrameworkCore.ExamManagement.UserExamQuestions
{
    public class UserExamQuestionRepository(IDbContextProvider<ExamDbContext> dbContextProvider)
        : EfCoreRepository<ExamDbContext, UserExamQuestion, Guid>(dbContextProvider), IUserExamQuestionRepository
    {
        // TODO:编写仓储代码
        public async Task<List<UserExamQuestionWithDetail>> GetListAsync(string? sorting = null, int skipCount = 0, int maxResultCount = Int32.MaxValue,
            Guid? userExamId = null, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var examQuestionQueryable = await GetQueryableAsync();
            var questionQueryable = dbContext.Set<Question>().AsQueryable();
            var questionAnswerQueryable = dbContext.Set<QuestionAnswer>().AsQueryable();

            var questions = await (from e in examQuestionQueryable
                                   join q in questionQueryable on e.QuestionId equals q.Id
                                   join a in questionAnswerQueryable on q.Id equals a.QuestionId into questionAnswers
                                   select new UserExamQuestionWithDetail()
                                   {
                                       Id = e.Id,
                                       Answers = e.Answers,
                                       QuestionId = q.Id,
                                       Question = q.Content,
                                       QuestionScore = e.QuestionScore,
                                       QuestionType = q.QuestionType,
                                       QuestionAnswers = questionAnswers
                                           .Select(qa => new UserExamQuestionWithDetail.QuestionAnswer()
                                           {
                                               Id = qa.Id,
                                               Content = qa.Content
                                           }).ToList()
                                   }).ToListAsync(cancellationToken: cancellationToken);
            return questions;
        }
    }
}