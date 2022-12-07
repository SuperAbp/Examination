using SuperAbp.Exam.QuestionManagement.Questions;
using System;

namespace SuperAbp.Exam.Admin.QuestionManagement.Questions;

public class QuestionCreateOrUpdateDtoBase
{
    public QuestionType QuestionType { get; set; }

    /// <summary>
    /// 题干
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// 解析
    /// </summary>
    public string Analysis { get; set; }

    /// <summary>
    /// 所属题库
    /// </summary>
    public Guid QuestionRepositoryId { get; set; }
}