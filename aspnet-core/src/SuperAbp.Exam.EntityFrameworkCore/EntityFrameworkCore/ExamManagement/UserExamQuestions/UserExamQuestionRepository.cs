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
    public class UserExamQuestionRepository(IDbContextProvider<ExamDbContext> dbContextProvider)
        : EfCoreRepository<ExamDbContext, UserExamQuestion, Guid>(dbContextProvider), IUserExamQuestionRepository
    {
        // TODO:编写仓储代码
        public async Task<List<UserExamQuestionWithDetails>> GetListAsync(string? sorting = null, int skipCount = 0, int maxResultCount = Int32.MaxValue,
            Guid? userExamId = null, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var examQuestionQueryable = await GetQueryableAsync();
            var userExamQueryable = dbContext.Set<UserExam>().AsQueryable().WhereIf(userExamId.HasValue, ue => ue.Id == userExamId);
            var questionQueryable = dbContext.Set<Question>().AsQueryable();
            var questionAnswerQueryable = dbContext.Set<QuestionAnswer>().AsQueryable();

            return await (from e in examQuestionQueryable
                          join ue in userExamQueryable on e.UserExamId equals ue.Id
                          join q in questionQueryable on e.QuestionId equals q.Id
                          join a in questionAnswerQueryable on q.Id equals a.QuestionId into questionAnswers
                          select new UserExamQuestionWithDetails()
                          {
                              Id = e.Id,
                              Answers = e.Answers,
                              Finished = ue.Finished,
                              QuestionId = q.Id,
                              Question = q.Content,
                              QuestionAnalysis = q.Analysis,
                              QuestionScore = e.QuestionScore,
                              QuestionType = q.QuestionType,
                              QuestionAnswers = questionAnswers
                                  .Select(qa => new UserExamQuestionWithDetails.QuestionAnswer()
                                  {
                                      Id = qa.Id,
                                      Content = qa.Content,
                                      Right = qa.Right
                                  }).ToList()
                          }).ToListAsync(GetCancellationToken(cancellationToken));
        }
    }
}