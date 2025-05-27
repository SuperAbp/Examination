using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SuperAbp.Exam.Admin.ExamManagement.UserExams;

/// <summary>
/// 用户考试管理
/// </summary>
public interface IUserExamAdminAppService : IApplicationService
{
    /// <summary>
    /// 列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PagedResultDto<UserExamWithUserDto>> GetListWithUserAsync(GetUserExamWithUsersInput input);

    /// <summary>
    /// 列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ListResultDto<UserExamListDto>> GetListAsync(GetUserExamsInput input);

    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    Task<UserExamDetailDto> GetAsync(Guid id);

    /// <summary>
    /// 题目审核
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="input"></param>
    /// <returns></returns>
    Task ReviewQuestionsAsync(Guid id, List<ReviewedQuestionDto> input);
}