using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

using SuperAbp.Exam.Admin.QuestionManagement.QuestionAnswers;
using System;

namespace SuperAbp.Exam.Admin.Controllers
{
    /// <summary>
    /// 答案
    /// </summary>
    [Route("api/question-management/question-answer")]
    public class QuestionAnswerController : ExamController, IQuestionAnswerAppService
    {
        private readonly IQuestionAnswerAppService _questionAnswerAppService;

        public QuestionAnswerController(IQuestionAnswerAppService questionAnswerAppService)
        {
            _questionAnswerAppService = questionAnswerAppService;
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns>结果</returns>
        [HttpGet]
        public virtual async Task<PagedResultDto<QuestionAnswerListDto>> GetListAsync(GetQuestionAnswersInput input)
        {
            return await _questionAnswerAppService.GetListAsync(input);
        }

        /// <summary>
        /// 获取修改
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [HttpGet("{id}/editor")]
        public virtual async Task<GetQuestionAnswerForEditorOutput> GetEditorAsync(Guid id)
        {
            return await _questionAnswerAppService.GetEditorAsync(id);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<QuestionAnswerListDto> CreateAsync(QuestionAnswerCreateDto input)
        {
            return await _questionAnswerAppService.CreateAsync(input);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public virtual async Task<QuestionAnswerListDto> UpdateAsync(Guid id, QuestionAnswerUpdateDto input)
        {
            return await _questionAnswerAppService.UpdateAsync(id, input);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _questionAnswerAppService.DeleteAsync(id);
        }
    }
}