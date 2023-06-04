using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SuperAbp.Exam.QuestionManagement.QuestionRepos
{
    /// <summary>
    /// 题库管理
    /// </summary>
    public interface IQuestionRepoAppService : IApplicationService
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns>结果</returns>
        Task<PagedResultDto<QuestionRepoListDto>> GetListAsync(GetQuestionReposInput input);

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id">题库Id</param>
        /// <returns></returns>
        Task<QuestionRepoDetailDto> GetAsync(Guid id);
    }
}