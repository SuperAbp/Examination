using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SuperAbp.Exam.Permissions;
using SuperAbp.Exam.QuestionManagement.QuestionCategories;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.QuestionManagement.QuestionCategories;

[Authorize(ExamPermissions.QuestionCategories.Default)]
public class QuestionCategoryAdminAppService(IQuestionCategoryRepository questionCategoryRepository) : ExamAppService, IQuestionCategoryAdminAppService
{
    public async Task<PagedResultDto<QuestionCategoryListDto>> GetListAsync(GetQuestionCategoriesInput input)
    {
        int count = await questionCategoryRepository.GetCountAsync(input.Name, input.ParentId);
        List<QuestionCategoryWithDetails> list = await questionCategoryRepository.GetListAsync(
            input.Sorting,
            input.SkipCount,
            input.MaxResultCount,
            input.Name,
            input.ParentId
        );
        List<QuestionCategoryListDto> dtos = ObjectMapper.Map<List<QuestionCategoryWithDetails>, List<QuestionCategoryListDto>>(list);
        return new PagedResultDto<QuestionCategoryListDto>(count, dtos);
    }

    public async Task<GetQuestionCategoryForEditorOutput> GetEditorAsync(Guid id)
    {
        QuestionCategory category = await questionCategoryRepository.GetAsync(id);
        return ObjectMapper.Map<QuestionCategory, GetQuestionCategoryForEditorOutput>(category);
    }

    [Authorize(ExamPermissions.Questions.Create)]
    public async Task<Guid> CreateAsync(QuestionCategoryCreateDto input)
    {
        QuestionCategory category = new(GuidGenerator.Create(), input.Name, input.ParentId);
        await questionCategoryRepository.InsertAsync(category);
        return category.Id;
    }

    [Authorize(ExamPermissions.Questions.Update)]
    public async Task UpdateAsync(Guid id, QuestionCategoryUpdateDto input)
    {
        QuestionCategory category = await questionCategoryRepository.GetAsync(id);
        category.Name = input.Name;
        category.ParentId = input.ParentId;
        await questionCategoryRepository.UpdateAsync(category);
    }

    [Authorize(ExamPermissions.Questions.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await questionCategoryRepository.DeleteAsync(id);
    }
}