using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

using SuperAbp.Exam.ExamManagement.Exams;
using System;

namespace SuperAbp.Exam.Controllers
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
    }
}