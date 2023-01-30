using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

using SuperAbp.Exam.Admin.ExamManagement.ExamRepos;
using System;

namespace SuperAbp.Exam.Admin.Controllers
{
    /// <summary>
    /// 考试题库
    /// </summary>
    [Route("api/examing-repository")]
    public class ExamingRepoController : ExamController, IExamingRepoAppService
    {
        private readonly IExamingRepoAppService _examRepoAppService;

        public ExamingRepoController(IExamingRepoAppService examRepoAppService)
        {
            _examRepoAppService = examRepoAppService;
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns>结果</returns>
        [HttpGet]
        public virtual async Task<PagedResultDto<ExamingRepoListDto>> GetListAsync(GetExamingReposInput input)
        {
            return await _examRepoAppService.GetListAsync(input);
        }

        /// <summary>
        /// 获取修改
        /// </summary>
        /// <param name="examingId">考试Id</param>
        /// <param name="questionRepositoryId">题库Id</param>
        /// <returns></returns>
        [HttpGet("{examingId}/repository/{questionRepositoryId}/editor")]
        public virtual async Task<GetExamingRepoForEditorOutput> GetEditorAsync(Guid examingId, Guid questionRepositoryId)
        {
            return await _examRepoAppService.GetEditorAsync(examingId, questionRepositoryId);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<ExamingRepoListDto> CreateAsync(ExamingRepoCreateDto input)
        {
            return await _examRepoAppService.CreateAsync(input);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="examingId">考试Id</param>
        /// <param name="questionRepositoryId">题库Id</param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("{examingId}/repository/{questionRepositoryId}")]
        public virtual async Task<ExamingRepoListDto> UpdateAsync(Guid examingId, Guid questionRepositoryId, ExamingRepoUpdateDto input)
        {
            return await _examRepoAppService.UpdateAsync(examingId, questionRepositoryId, input);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="examingId">考试Id</param>
        /// <param name="questionRepositoryId">题库Id</param>
        /// <returns></returns>
        [HttpDelete("{examingId}/repository/{questionRepositoryId}")]
        public virtual async Task DeleteAsync(Guid examingId, Guid questionRepositoryId)
        {
            await _examRepoAppService.DeleteAsync(examingId, questionRepositoryId);
        }
    }
}