using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using System;
using SuperAbp.Exam.Admin.QuestionManagement.QuestionBanks;

namespace SuperAbp.Exam.Admin.Controllers
{
    /// <summary>
    /// 题库
    /// </summary>
    [Route("api/question-management/question-bank")]
    public class QuestionBankController(IQuestionBankAdminAppService questionRepoAppService) : ExamController, IQuestionBankAdminAppService
    {
        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public virtual async Task<QuestionBankDetailDto> GetAsync(Guid id)
        {
            return await questionRepoAppService.GetAsync(id);
        }

        /// <summary>
        /// 获取题目数量
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/question-count")]
        public async Task<QuestionBankCountDto> GetQuestionCountAsync(Guid id)
        {
            return await questionRepoAppService.GetQuestionCountAsync(id);
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns>结果</returns>
        [HttpGet]
        public virtual async Task<PagedResultDto<QuestionBankListDto>> GetListAsync(GetQuestionBanksInput input)
        {
            return await questionRepoAppService.GetListAsync(input);
        }

        /// <summary>
        /// 获取修改
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [HttpGet("{id}/editor")]
        public virtual async Task<GetQuestionBankForEditorOutput> GetEditorAsync(Guid id)
        {
            return await questionRepoAppService.GetEditorAsync(id);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<QuestionBankListDto> CreateAsync(QuestionBankCreateDto input)
        {
            return await questionRepoAppService.CreateAsync(input);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public virtual async Task<QuestionBankListDto> UpdateAsync(Guid id, QuestionBankUpdateDto input)
        {
            return await questionRepoAppService.UpdateAsync(id, input);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public virtual async Task DeleteAsync(Guid id)
        {
            await questionRepoAppService.DeleteAsync(id);
        }
    }
}