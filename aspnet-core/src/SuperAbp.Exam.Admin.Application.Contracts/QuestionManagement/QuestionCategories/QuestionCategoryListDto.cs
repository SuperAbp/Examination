using System;

namespace SuperAbp.Exam.Admin.QuestionManagement.QuestionCategories;

public class QuestionCategoryListDto
{
    public Guid Id { get; set; }
    public string? ParentName { get; set; }

    public required string Name { get; set; }
}