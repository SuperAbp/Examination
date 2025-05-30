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
using SuperAbp.Exam.QuestionManagement.QuestionAnswers;
using SuperAbp.Exam.QuestionManagement.Questions;

namespace SuperAbp.Exam.EntityFrameworkCore.ExamManagement.UserExamQuestions
{
    public class UserExamQuestionRepository(IDbContextProvider<IExamDbContext> dbContextProvider)
        : EfCoreRepository<IExamDbContext, UserExamQuestion, Guid>(dbContextProvider), IUserExamQuestionRepository
    {
        // TODO:编写仓储代码
        public async Task<List<UserExamQuestionWithDetails>> GetListAsync(Guid userExamId, string? sorting = null, int skipCount = 0, int maxResultCount = Int32.MaxValue,
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var examQuestionQueryable = await GetQueryableAsync();
            var userExamQueryable = dbContext.Set<UserExam>().AsQueryable();
            var questionQueryable = dbContext.Set<Question>().AsQueryable();
            var questionAnswerQueryable = dbContext.Set<QuestionAnswer>().AsQueryable();

            return await (from e in examQuestionQueryable
                          join ue in userExamQueryable on e.UserExamId equals ue.Id
                          join q in questionQueryable on e.QuestionId equals q.Id
                          join a in questionAnswerQueryable on q.Id equals a.QuestionId into questionAnswers
                          where e.UserExamId == userExamId
                          select new UserExamQuestionWithDetails()
                          {
                              Id = e.Id,
                              Answers = e.Answers,
                              Right = e.Right,
                              Score = e.Score,
                              QuestionId = q.Id,
                              Question = q.Content,
                              QuestionAnalysis = q.Analysis,
                              QuestionScore = e.QuestionScore,
                              QuestionType = q.QuestionType,
                              QuestionReviewReason = e.Reason,
                              QuestionAnswers = questionAnswers
                                  .Select(qa => new UserExamQuestionWithDetails.QuestionAnswer()
                                  {
                                      Id = qa.Id,
                                      Content = qa.Content,
                                      Right = qa.Right
                                  }).ToList()
                          }).ToListAsync(cancellationToken);
        }
    }
}