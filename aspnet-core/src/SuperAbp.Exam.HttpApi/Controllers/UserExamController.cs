using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

using SuperAbp.Exam.ExamManagement.UserExams;

namespace SuperAbp.Exam.Controllers
{
    /// <summary>
    /// 用户考试
    /// </summary>
    [Route("api/userExam")]
    public class UserExamController : ExamController, IUserExamAppService
    {
        private readonly IUserExamAppService _userExamAppService;

        public UserExamController(IUserExamAppService userExamAppService)
        {
            _userExamAppService = userExamAppService;
        }

        /// <summary>
        /// 获取未完成的考试
        /// </summary>
        /// <returns>考试Id（没有则为null）</returns>
        [HttpGet("unfinished")]
        public async Task<Guid?> GetUnfinishedAsync()
        {
            return await _userExamAppService.GetUnfinishedAsync();
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public virtual async Task<UserExamDetailDto> GetAsync(Guid id)
        {
            return await _userExamAppService.GetAsync(id);
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns>结果</returns>
        [HttpGet]
        public virtual async Task<PagedResultDto<UserExamListDto>> GetListAsync(GetUserExamsInput input)
        {
            return await _userExamAppService.GetListAsync(input);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual async Task<UserExamListDto> CreateAsync(UserExamCreateDto input)
        {
            return await _userExamAppService.CreateAsync(input);
        }

        /// <summary>
        /// 完成考试
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("{id}/finished")]
        public async Task FinishedAsync(Guid id)
        {
            await _userExamAppService.FinishedAsync(id);
        }
    }
}