using System;

namespace SuperAbp.Exam.ExamManagement.UserExams;

public class UserExamListDto
{
    public Guid ExamId { get; set; }

    /// <summary>
    /// 总分
    /// </summary>
    public decimal TotalScore { get; set; }

    /// <summary>
    /// 考试名称
    /// </summary>
    public string ExamName { get; set; }

    /// <summary>
    /// 时长
    /// </summary>
    public long Minutes { get; set; }
}