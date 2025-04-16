using System;

namespace SuperAbp.Exam.QuestionManagement.QuestionCategories;

public class QuestionCategoryWithDetails
{
    public Guid Id { get; set; }
    public required string Name { get; set; }

    public string? ParentName { get; set; }

    public DateTime CreationTime { get; set; }
}