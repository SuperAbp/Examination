using Microsoft.AspNetCore.Mvc;
using SuperAbp.Exam.ExamManagement.Exams;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Controllers;

/// <summary>
/// 考试管理
/// </summary>
[Route("api/exams")]
public class ExaminationController(IExaminationAppService examinationAppService) : ExamController, IExaminationAppService
{
    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public virtual async Task<ExamDetailDto> GetAsync(Guid id)
    {
        return await examinationAppService.GetAsync(id);
    }

    /// <summary>
    /// 列表
    /// </summary>
    /// <param name="input">查询条件</param>
    /// <returns>结果</returns>
    [HttpGet]
    public virtual async Task<PagedResultDto<ExamListDto>> GetListAsync(GetExamsInput input)
    {
        return await examinationAppService.GetListAsync(input);
    }

    /// <summary>
    /// 获取考试排名列表
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns>排名列表</returns>
    [HttpGet("{id}/ranking")]
    public async Task<List<ExamRankingDto>> GetRankingListAsync(Guid id)
    {
        return await examinationAppService.GetRankingListAsync(id);
    }
}