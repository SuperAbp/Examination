using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SuperAbp.Exam.KnowledgePoints;
using SuperAbp.Exam.Permissions;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.KnowledgePoints;

[Authorize(ExamPermissions.KnowledgePoints.Default)]
public class KnowledgePointAdminAppService(IKnowledgePointRepository knowledgePointRepository) : ExamAppService, IKnowledgePointAdminAppService
{
    public async Task<ListResultDto<KnowledgePointListDto>> GetAllAsync(GetKnowledgePointsInput input)
    {
        List<KnowledgePoint> knowledgePoints = await knowledgePointRepository.GetListAsync(input.Name);
        return new ListResultDto<KnowledgePointListDto>(ObjectMapper.Map<List<KnowledgePoint>, List<KnowledgePointListDto>>(knowledgePoints));
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