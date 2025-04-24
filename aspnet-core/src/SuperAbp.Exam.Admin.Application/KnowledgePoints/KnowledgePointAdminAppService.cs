using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SuperAbp.Exam.KnowledgePoints;
using SuperAbp.Exam.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.ObjectMapping;

namespace SuperAbp.Exam.Admin.KnowledgePoints;

[Authorize(ExamPermissions.KnowledgePoints.Default)]
public class KnowledgePointAdminAppService(IKnowledgePointRepository knowledgePointRepository) : ExamAppService, IKnowledgePointAdminAppService
{
    public async Task<ListResultDto<KnowledgePointNodeDto>> GetAllAsync(GetKnowledgePointsInput input)
    {
        List<KnowledgePoint> knowledgePoints = await knowledgePointRepository.GetListAsync(input.Name);
        List<KnowledgePointNodeDto> dtos = BuildTree(knowledgePoints);
        return new ListResultDto<KnowledgePointNodeDto>(dtos);
    }

    private List<KnowledgePointNodeDto> BuildTree(List<KnowledgePoint> knowledgePoints)
    {
        var groupedByParentId = knowledgePoints
            .GroupBy(kp => kp.ParentId ?? Guid.Empty)
            .ToDictionary(g => g.Key, g => g.ToList());

        return BuildTreeNodes(Guid.Empty, groupedByParentId);
    }

    private List<KnowledgePointNodeDto> BuildTreeNodes(Guid parentId, Dictionary<Guid, List<KnowledgePoint>> groupedByParentId)
    {
        if (!groupedByParentId.TryGetValue(parentId, out var children))
        {
            return new List<KnowledgePointNodeDto>();
        }

        return children.Select(kp => new KnowledgePointNodeDto
        {
            Id = kp.Id,
            Name = kp.Name,
            Children = BuildTreeNodes(kp.Id, groupedByParentId) // Recursively build child nodes
        }).ToList();
    }

    public async Task<GetKnowledgePointForEditorOutput> GetEditorAsync(Guid id)
    {
        KnowledgePoint category = await knowledgePointRepository.GetAsync(id);
        return ObjectMapper.Map<KnowledgePoint, GetKnowledgePointForEditorOutput>(category);
    }

    [Authorize(ExamPermissions.KnowledgePoints.Create)]
    public async Task<Guid> CreateAsync(KnowledgePointCreateDto input)
    {
        KnowledgePoint category = new(GuidGenerator.Create(), input.Name, input.ParentId);
        await knowledgePointRepository.InsertAsync(category);
        return category.Id;
    }

    [Authorize(ExamPermissions.KnowledgePoints.Update)]
    public async Task UpdateAsync(Guid id, KnowledgePointUpdateDto input)
    {
        KnowledgePoint category = await knowledgePointRepository.GetAsync(id);
        category.Name = input.Name;
        category.ParentId = input.ParentId;
        await knowledgePointRepository.UpdateAsync(category);
    }

    [Authorize(ExamPermissions.KnowledgePoints.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await knowledgePointRepository.DeleteAsync(id);
    }
}