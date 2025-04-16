using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.QuestionManagement.QuestionCategories;

public class GetQuestionCategoriesInput : PagedAndSortedResultRequestDto
{
    public Guid? ParentId { get; set; }
    public string? Name { get; set; }
}