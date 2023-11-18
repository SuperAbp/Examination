using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SuperAbp.Exam.ExamManagement.UserExams
{
    /// <summary>
    /// 用户考试管理
    /// </summary>
    public interface IUserExamAppService : IApplicationService
    {
        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<UserExamDetailDto> GetAsync(Guid id);

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns>结果</returns>
        Task<PagedResultDto<UserExamListDto>> GetListAsync(GetUserExamsInput input);

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<UserExamListDto> CreateAsync(UserExamCreateDto input);
    }
}