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
        /// 获取未完成的考试
        /// </summary>
        /// <returns>考试Id（没有则为null）</returns>
        Task<Guid?> GetUnfinishedAsync();

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

        /// <summary>
        /// 完成考试
        /// </summary>
        /// <param name="id"></param>
        Task FinishedAsync(Guid id);
    }
}