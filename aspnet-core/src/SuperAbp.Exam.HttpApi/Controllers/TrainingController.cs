using Microsoft.AspNetCore.Mvc;
using SuperAbp.Exam.TrainingManagement;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Controllers;

/// <summary>
/// 训练
/// </summary>
[Route("api/training")]
public class TrainingController : ExamController, ITrainingAppService
{
    private readonly ITrainingAppService _trainingAppService;

    public TrainingController(ITrainingAppService trainingAppService)
    {
        _trainingAppService = trainingAppService;
    }

    /// <summary>
    /// 列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ListResultDto<TrainingListDto>> GetListAsync(GetTrainsInput input)
    {
        return await _trainingAppService.GetListAsync(input);
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<TrainingListDto> CreateAsync(TrainingCreateDto input)
    {
        return await _trainingAppService.CreateAsync(input);
    }

    /// <summary>
    /// 设置结果
    /// </summary>
    /// <param name="id"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    [HttpPatch]
    public async Task SetIsRightAsync(Guid id, bool right)
    {
        await _trainingAppService.SetIsRightAsync(id, right);
    }
}