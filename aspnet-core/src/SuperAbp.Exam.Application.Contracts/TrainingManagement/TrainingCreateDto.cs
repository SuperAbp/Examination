using System;

namespace SuperAbp.Exam.TrainingManagement;

public class TrainingCreateDto
{
    /// <summary>
    /// 题库Id
    /// </summary>
    public Guid QuestionRepositoryId { get; set; }

    /// <summary>
    /// 题目Id
    /// </summary>
    public Guid QuestionId { get; set; }

    public TrainingSource TrainingSource { get; set; }

    public bool Right { get; set; }
}