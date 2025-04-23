using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SuperAbp.Exam.Admin.KnowledgePoints;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.Controllers;

/// <summary>
/// 知识点
/// </summary>
[Route("knowledge-point")]
public class KnowledgePointController(IKnowledgePointAdminAppService knowledgePointAppService)
    : ExamController, IKnowledgePointAdminAppService
{
    /// <summary>
    /// 所有
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ListResultDto<KnowledgePointListDto>> GetAllAsync(GetKnowledgePointsInput input)
    {
        return await knowledgePointAppService.GetAllAsync(input);
    }

    /// <summary>
    /// 获取修改
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    [HttpGet("{id}/editor")]
    public async Task<GetKnowledgePointForEditorOutput> GetEditorAsync(Guid id)
    {
        return await knowledgePointAppService.GetEditorAsync(id);
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<Guid> CreateAsync(KnowledgePointCreateDto input)
    {
        return await knowledgePointAppService.CreateAsync(input);
    }

    /// <summary>
    /// 编辑
    /// </summary>
    /// <param name="id">主键</param>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task UpdateAsync(Guid id, KnowledgePointUpdateDto input)
    {
        await knowledgePointAppService.UpdateAsync(id, input);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id">主键</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task DeleteAsync(Guid id)
    {
        await knowledgePointAppService.DeleteAsync(id);
    }
}