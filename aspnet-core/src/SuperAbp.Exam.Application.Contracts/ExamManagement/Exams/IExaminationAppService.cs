using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SuperAbp.Exam.ExamManagement.Exams
{
    /// <summary>
    /// 考试管理
    /// </summary>
    public interface IExaminationAppService : IApplicationService
    {
        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<ExamDetailDto> GetAsync(Guid id);

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns>结果</returns>
        Task<PagedResultDto<ExamListDto>> GetListAsync(GetExamsInput input);

        /// <summary>
        /// 获取考试排名列表
        /// </summary>
        /// <param name="examId">考试ID</param>
        /// <returns>排名列表</returns>
        Task<ListResultDto<ExamRankingDto>> GetRankingListAsync(Guid examId);
    }
}