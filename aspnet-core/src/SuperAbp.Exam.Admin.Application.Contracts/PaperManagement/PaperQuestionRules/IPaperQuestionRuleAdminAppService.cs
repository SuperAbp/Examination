using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SuperAbp.Exam.Admin.PaperManagement.PaperQuestionRules
{
    /// <summary>
    /// 考试题库管理
    /// </summary>
    public interface IPaperQuestionRuleAdminAppService : IApplicationService
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns>结果</returns>
        Task<PagedResultDto<PaperQuestionRuleListDto>> GetListAsync(GetPaperQuestionRulesInput input);

        /// <summary>
        /// 获取修改
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        Task<GetPaperQuestionRuleForEditorOutput> GetEditorAsync(Guid id);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(Guid id);
    }
}