using System;
using System.Threading.Tasks;
using SuperAbp.Exam.QuestionManagement.Questions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SuperAbp.Exam.QuestionManagement.QuestionBanks
{
    /// <summary>
    /// 题库管理
    /// </summary>
    public interface IQuestionBankAppService : IApplicationService
    {
        /// <summary>
        /// 题型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ListResultDto<QuestionType>> GetQuestionTypesAsync(Guid id);

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns>结果</returns>
        Task<PagedResultDto<QuestionBankListDto>> GetListAsync(GetQuestionBanksInput input);

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id">题库Id</param>
        /// <returns></returns>
        Task<QuestionBankDetailDto> GetAsync(Guid id);
    }
}