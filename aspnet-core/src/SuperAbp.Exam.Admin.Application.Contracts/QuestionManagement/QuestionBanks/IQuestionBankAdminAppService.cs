using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SuperAbp.Exam.Admin.QuestionManagement.QuestionBanks
{
    /// <summary>
    /// 题库管理
    /// </summary>
    public interface IQuestionBankAdminAppService : IApplicationService
    {
        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<QuestionBankDetailDto> GetAsync(Guid id);

        /// <summary>
        /// 获取题目数量
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<QuestionBankCountDto> GetQuestionCountAsync(Guid id);

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns>结果</returns>
        Task<PagedResultDto<QuestionBankListDto>> GetListAsync(GetQuestionBanksInput input);

        /// <summary>
        /// 获取修改
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<GetQuestionBankForEditorOutput> GetEditorAsync(Guid id);

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<QuestionBankListDto> CreateAsync(QuestionBankCreateDto input);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<QuestionBankListDto> UpdateAsync(Guid id, QuestionBankUpdateDto input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task DeleteAsync(Guid id);
    }
}