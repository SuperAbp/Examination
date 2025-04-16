using System;

namespace SuperAbp.Exam.Admin.QuestionManagement.QuestionCategories;

public class GetQuestionCategoryForEditorOutput
{
    public Guid? ParentId { get; set; }

    public required string Name { get; set; }
}