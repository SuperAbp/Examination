using System;

namespace SuperAbp.Exam.ExamManagement.Exams.Events;

/// <summary>
/// 考试评分完成事件
/// </summary>
public record ExamGradingCompletedEvent(
    Guid ExamId,
    string ExamName
);