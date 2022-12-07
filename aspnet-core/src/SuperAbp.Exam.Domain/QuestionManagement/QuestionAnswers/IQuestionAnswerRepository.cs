using System;
using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.QuestionManagement.QuestionAnswers
{
    /// <summary>
    /// 答案
    /// </summary>
    public interface IQuestionAnswerRepository : IRepository<QuestionAnswer, Guid>
    {
    }
}