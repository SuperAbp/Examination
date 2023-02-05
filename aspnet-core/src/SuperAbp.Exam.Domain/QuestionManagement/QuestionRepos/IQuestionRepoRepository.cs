using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.QuestionManagement.QuestionRepos
{
    /// <summary>
    /// 题库
    /// </summary>
    public interface IQuestionRepoRepository : IRepository<QuestionRepo, Guid>
    {
        /// <summary>
        /// 获取标题
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<string> FindTitleAsync(Guid id);

    }
}