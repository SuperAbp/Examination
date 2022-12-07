using System;
using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.QuestionManagement.QuestionRepos
{
    /// <summary>
    /// 题库
    /// </summary>
    public interface IQuestionRepoRepository : IRepository<QuestionRepo, Guid>
    {
    }
}