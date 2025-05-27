using SuperAbp.Exam.Admin.ExamManagement.UserExams;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.Controllers;

/// <summary>
/// 用户考试管理
/// </summary>
/// <param name="userExamAdminAppService"></param>
[Route("api/user-exam")]
public class UserExamController(IUserExamAdminAppService userExamAdminAppService) : ExamController, IUserExamAdminAppService
{
    protected IUserExamAdminAppService UserExamAdminAppService { get; } = userExamAdminAppService;

    /// <summary>
    /// 用户列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("user")]
    public async Task<PagedResultDto<UserExamWithUserDto>> GetListWithUserAsync(GetUserExamWithUsersInput input)
    {
        return await UserExamAdminAppService.GetListWithUserAsync(input);
    }

    /// <summary>
    /// 列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ListResultDto<UserExamListDto>> GetListAsync(GetUserExamsInput input)
    {
        return await UserExamAdminAppService.GetListAsync(input);
    }

    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<UserExamDetailDto> GetAsync(Guid id)
    {
        return await UserExamAdminAppService.GetAsync(id);
    }

    /// <summary>
    /// 题目审核
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPatch("review")]
    public async Task ReviewQuestionsAsync(Guid id, List<ReviewedQuestionDto> input)
    {
        await UserExamAdminAppService.ReviewQuestionsAsync(id, input);
    }
}