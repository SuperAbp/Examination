using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SuperAbp.Exam.Admin.ExamManagement.ExamRepos
{
    /// <summary>
    /// 考试题库管理
    /// </summary>
    public interface IExamingRepoAppService : IApplicationService
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns>结果</returns>
        Task<PagedResultDto<ExamingRepoListDto>> GetListAsync(GetExamingReposInput input);

        /// <summary>
        /// 获取修改
        /// </summary>
        /// <param name="examingId">考试Id</param>
        /// <param name="questionRepositoryId">题库Id</param>
        /// <returns></returns>
        Task<GetExamingRepoForEditorOutput> GetEditorAsync(Guid examingId, Guid questionRepositoryId);

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ExamingRepoListDto> CreateAsync(ExamingRepoCreateDto input);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="examingId">考试Id</param>
        /// <param name="questionRepositoryId">题库Id</param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ExamingRepoListDto> UpdateAsync(Guid examingId, Guid questionRepositoryId, ExamingRepoUpdateDto input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="examingId">考试Id</param>
        /// <param name="questionRepositoryId">题库Id</param>
        /// <returns></returns>
        Task DeleteAsync(Guid examingId, Guid questionRepositoryId);
    }
}