using SuperAbp.Exam.QuestionManagement.Questions;
using System;

namespace SuperAbp.Exam.Admin.QuestionManagement.Questions;

public class QuestionImportDto
{
    /// <summary>
    /// 所属题库
    /// </summary>
    public Guid QuestionRepositoryId { get; set; }

    public QuestionType QuestionType { get; set; }

    public string Content { get; set; }
}