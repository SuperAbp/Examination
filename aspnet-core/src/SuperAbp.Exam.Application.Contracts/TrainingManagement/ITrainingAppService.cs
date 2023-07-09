using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SuperAbp.Exam.TrainingManagement;

/// <summary>
/// 训练
/// </summary>
public interface ITrainingAppService : IApplicationService
{
    /// <summary>
    /// 列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<ListResultDto<TrainingListDto>> GetListAsync(GetTrainsInput input);

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<TrainingListDto> CreateAsync(TrainingCreateDto input);

    /// <summary>
    /// 设置结果
    /// </summary>
    /// <param name="id"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    Task SetIsRightAsync(Guid id, bool right);
}