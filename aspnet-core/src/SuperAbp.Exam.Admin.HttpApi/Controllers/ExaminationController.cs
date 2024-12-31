using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

using SuperAbp.Exam.Admin.ExamManagement.Exams;
using System;

namespace SuperAbp.Exam.Admin.Controllers
{
    /// <summary>
    /// 考试
    /// </summary>
    [Route("api/exam")]
    public class ExaminationController : ExamController, IExaminationAdminAppService
    {
        private readonly IExaminationAdminAppService _examAppService;

        public ExaminationController(IExaminationAdminAppService examAppService)
        {
            _examAppService = examAppService;
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public virtual async Task<ExamDetailDto> GetAsync(Guid id)
        {
            return await _examAppService.GetAsync(id);
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns>结果</returns>
        [HttpGet]
        public virtual async Task<PagedResultDto<ExamListDto>> GetListAsync(GetExamsInput input)
        {
            return await _examAppService.GetListAsync(input);
        }

        /// <summary>
        /// 获取修改
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [HttpGet("{id}/editor")]
        public virtual async Task<GetExamForEditorOutput> GetEditorAsync(Guid id)
        {
            return await _examAppService.GetEditorAsync(id);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<ExamListDto> CreateAsync(ExamCreateDto input)
        {
            return await _examAppService.CreateAsync(input);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public virtual async Task<ExamListDto> UpdateAsync(Guid id, ExamUpdateDto input)
        {
            return await _examAppService.UpdateAsync(id, input);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _examAppService.DeleteAsync(id);
        }
    }
}