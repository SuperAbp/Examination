using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using System;
using SuperAbp.Exam.Admin.PaperManagement.PaperRepos;

namespace SuperAbp.Exam.Admin.Controllers
{
    /// <summary>
    /// 考试题库
    /// </summary>
    [Route("api/paper-repository")]
    public class PaperRepoController : ExamController, IPaperRepoAppService
    {
        private readonly IPaperRepoAppService _examRepoAppService;

        public PaperRepoController(IPaperRepoAppService examRepoAppService)
        {
            _examRepoAppService = examRepoAppService;
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns>结果</returns>
        [HttpGet]
        public virtual async Task<PagedResultDto<PaperRepoListDto>> GetListAsync(GetPaperReposInput input)
        {
            return await _examRepoAppService.GetListAsync(input);
        }

        /// <summary>
        /// 获取修改
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public virtual async Task<GetPaperRepoForEditorOutput> GetEditorAsync(Guid id)
        {
            return await _examRepoAppService.GetEditorAsync(id);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PaperRepoListDto> CreateAsync(PaperRepoCreateDto input)
        {
            return await _examRepoAppService.CreateAsync(input);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public async Task<PaperRepoListDto> UpdateAsync(Guid id, PaperRepoUpdateDto input)
        {
            return await _examRepoAppService.UpdateAsync(id, input);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _examRepoAppService.DeleteAsync(id);
        }
    }
}