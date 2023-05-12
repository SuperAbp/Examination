using System;
using System.Threading.Tasks;
using SuperAbp.Exam.Admin.ExamManagement.Exams;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SuperAbp.Exam.ExamManagement.Exams
{
    /// <summary>
    /// 考试管理
    /// </summary>
    public interface IExamingAppService : IApplicationService
    {
        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<ExamingDetailDto> GetAsync(Guid id);

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns>结果</returns>
        Task<PagedResultDto<ExamingListDto>> GetListAsync(GetExamingsInput input);
    }
}