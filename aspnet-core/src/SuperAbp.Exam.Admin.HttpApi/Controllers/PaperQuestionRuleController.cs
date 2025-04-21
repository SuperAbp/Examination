using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using System;
using SuperAbp.Exam.Admin.PaperManagement.PaperQuestionRules;

namespace SuperAbp.Exam.Admin.Controllers
{
    /// <summary>
    /// 考试题库
    /// </summary>
    [Route("api/paper-question-rule")]
    public class PaperQuestionRuleController(IPaperQuestionRuleAdminAppService paperQuestionRuleAppService)
        : ExamController, IPaperQuestionRuleAdminAppService
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns>结果</returns>
        [HttpGet]
        public virtual async Task<PagedResultDto<PaperQuestionRuleListDto>> GetListAsync(GetPaperQuestionRulesInput input)
        {
            return await paperQuestionRuleAppService.GetListAsync(input);
        }

        /// <summary>
        /// 获取修改
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public virtual async Task<GetPaperQuestionRuleForEditorOutput> GetEditorAsync(Guid id)
        {
            return await paperQuestionRuleAppService.GetEditorAsync(id);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public virtual async Task DeleteAsync(Guid id)
        {
            await paperQuestionRuleAppService.DeleteAsync(id);
        }
    }
}