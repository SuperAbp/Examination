using System;

namespace SuperAbp.Exam.QuestionManagement.Questions;

/// <summary>
/// 题目已删除事件
/// </summary>
public class QuestionDeletedEvent
{
    public Guid QuestionId { get; set; }
    public Guid? TenantId { get; set; }
}
