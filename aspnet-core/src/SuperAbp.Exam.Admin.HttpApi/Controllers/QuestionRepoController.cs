using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

using SuperAbp.Exam.Admin.QuestionManagement.QuestionRepos;
using System;

namespace SuperAbp.Exam.Admin.Controllers
{
    /// <summary>
    /// 题库
    /// </summary>
    [Route("api/question-management/question-repository")]
    public class QuestionRepoController : ExamController, IQuestionRepoAdminAppService
    {
        private readonly IQuestionRepoAdminAppService _questionRepoAppService;

        public QuestionRepoController(IQuestionRepoAdminAppService questionRepoAppService)
        {
            _questionRepoAppService = questionRepoAppService;
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public virtual async Task<QuestionRepoDetailDto> GetAsync(Guid id)
        {
            return await _questionRepoAppService.GetAsync(id);
        }

        /// <summary>
        /// 获取题目数量
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/question-count")]
        public async Task<QuestionRepoCountDto> GetQuestionCountAsync(Guid id)
        {
            return await _questionRepoAppService.GetQuestionCountAsync(id);
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns>结果</returns>
        [HttpGet]
        public virtual async Task<PagedResultDto<QuestionRepoListDto>> GetListAsync(GetQuestionReposInput input)
        {
            return await _questionRepoAppService.GetListAsync(input);
        }

        /// <summary>
        /// 获取修改
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [HttpGet("{id}/editor")]
        public virtual async Task<GetQuestionRepoForEditorOutput> GetEditorAsync(Guid id)
        {
            return await _questionRepoAppService.GetEditorAsync(id);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<QuestionRepoListDto> CreateAsync(QuestionRepoCreateDto input)
        {
            return await _questionRepoAppService.CreateAsync(input);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public virtual async Task<QuestionRepoListDto> UpdateAsync(Guid id, QuestionRepoUpdateDto input)
        {
            return await _questionRepoAppService.UpdateAsync(id, input);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _questionRepoAppService.DeleteAsync(id);
        }
    }
}