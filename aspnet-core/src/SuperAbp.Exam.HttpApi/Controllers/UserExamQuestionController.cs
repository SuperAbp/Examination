using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

using SuperAbp.Exam.ExamManagement.UserExamQuestions;

namespace SuperAbp.Exam.Controllers
{
    /// <summary>
    /// 用户考题
    /// </summary>
    [Route("api/userExamQuestion")]
    public class UserExamQuestionController(IUserExamQuestionAppService userExamQuestionAppService)
        : ExamController, IUserExamQuestionAppService
    {
        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public virtual async Task<UserExamQuestionDetailDto> GetAsync(Guid id)
        {
            return await userExamQuestionAppService.GetAsync(id);
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns>结果</returns>
        [HttpGet]
        public virtual async Task<PagedResultDto<UserExamQuestionListDto>> GetListAsync(GetUserExamQuestionsInput input)
        {
            return await userExamQuestionAppService.GetListAsync(input);
        }

        /// <summary>
        /// 获取修改
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [HttpGet("{id}/editor")]
        public virtual async Task<GetUserExamQuestionForEditorOutput> GetEditorAsync(Guid id)
        {
            return await userExamQuestionAppService.GetEditorAsync(id);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<UserExamQuestionListDto> CreateAsync(UserExamQuestionCreateDto input)
        {
            return await userExamQuestionAppService.CreateAsync(input);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public virtual async Task<UserExamQuestionListDto> AnswerAsync(Guid id, UserExamQuestionAnswerDto input)
        {
            return await userExamQuestionAppService.AnswerAsync(id, input);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public virtual async Task DeleteAsync(Guid id)
        {
            await userExamQuestionAppService.DeleteAsync(id);
        }
    }
}