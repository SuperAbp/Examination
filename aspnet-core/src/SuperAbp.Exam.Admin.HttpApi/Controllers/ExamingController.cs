using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

using SuperAbp.Exam.Admin.ExamManagement.Exams;

namespace SuperAbp.Exam.Admin.Controllers
{
    /// <summary>
    /// 考试
    /// </summary>
    [Route("api/examing")]
    public class ExamingController : ExamController, IExamingAppService
    {
        private readonly IExamingAppService _examingAppService;

        public ExamingController(IExamingAppService examingAppService)
        {
            _examingAppService = examingAppService;
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public virtual async Task<ExamingDetailDto> GetAsync(Guid id)
        {
            return await _examingAppService.GetAsync(id);
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns>结果</returns>
        [HttpGet]
        public virtual async Task<PagedResultDto<ExamingListDto>> GetListAsync(GetExamingsInput input)
        {
            return await _examingAppService.GetListAsync(input);
        }

        /// <summary>
        /// 获取修改
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [HttpGet("{id}/editor")]
        public virtual async Task<GetExamingForEditorOutput> GetEditorAsync(Guid id)
        {
            return await _examingAppService.GetEditorAsync(id);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<ExamingListDto> CreateAsync(ExamingCreateDto input)
        {
            return await _examingAppService.CreateAsync(input);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public virtual async Task<ExamingListDto> UpdateAsync(Guid id, ExamingUpdateDto input)
        {
            return await _examingAppService.UpdateAsync(id, input);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _examingAppService.DeleteAsync(id);
        }
    }
}
