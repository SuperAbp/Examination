using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

using SuperAbp.Exam.Admin.ExamManagement.Exams;
using System;
using System.Collections.Generic;

namespace SuperAbp.Exam.Admin.Controllers;

/// <summary>
/// 考试
/// </summary>
[Route("api/exam")]
public class ExaminationController(IExaminationAdminAppService examAppService)
    : ExamController, IExaminationAdminAppService
{
    protected IExaminationAdminAppService ExamAppService { get; } = examAppService;

    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public virtual async Task<ExamDetailDto> GetAsync(Guid id)
    {
        return await ExamAppService.GetAsync(id);
    }

    /// <summary>
    /// 列表
    /// </summary>
    /// <param name="input">查询条件</param>
    /// <returns>结果</returns>
    [HttpGet]
    public virtual async Task<PagedResultDto<ExamListDto>> GetListAsync(GetExamsInput input)
    {
        return await ExamAppService.GetListAsync(input);
    }

    /// <summary>
    /// 考试排名列表
    /// </summary>
    /// <param name="examId">考试ID</param>
    /// <returns>排名列表</returns>
    [HttpGet("{examId}/user-exams")]
    public async Task<ListResultDto<ExamUserExamDto>> GetExamUserExamsAsync(Guid examId)
    {
        return await ExamAppService.GetExamUserExamsAsync(examId);
    }

    /// <summary>
    /// 获取修改
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    [HttpGet("{id}/editor")]
    public virtual async Task<GetExamForEditorOutput> GetEditorAsync(Guid id)
    {
        return await ExamAppService.GetEditorAsync(id);
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public virtual async Task<ExamListDto> CreateAsync(ExamCreateDto input)
    {
        return await ExamAppService.CreateAsync(input);
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
        return await ExamAppService.UpdateAsync(id, input);
    }

    /// <summary>
    /// 取消考试
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPatch("{id}/cancel")]
    public virtual async Task CancelAsync(Guid id)
    {
        await ExamAppService.CancelAsync(id);
    }

    /// <summary>
    /// 发布考试
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPatch("{id}/publish")]
    public virtual async Task PublishAsync(Guid id)
    {
        await ExamAppService.PublishAsync(id);
    }

    /// <summary>
    /// 终止考试
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPatch("{id}/terminate")]
    public async Task TerminateAsync(Guid id)
    {
        await ExamAppService.TerminateAsync(id);
    }

    /// <summary>
    /// 完成考试
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>

    [HttpPatch("{id}/complete")]
    public async Task CompleteAsync(Guid id)
    {
        await ExamAppService.CompleteAsync(id);
    }

    /// <summary>
    /// 作废考试
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPatch("{id}/invalidate")]
    public async Task InvalidateAsync(Guid id)
    {
        await ExamAppService.InvalidateAsync(id);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public virtual async Task DeleteAsync(Guid id)
    {
        await ExamAppService.DeleteAsync(id);
    }
}