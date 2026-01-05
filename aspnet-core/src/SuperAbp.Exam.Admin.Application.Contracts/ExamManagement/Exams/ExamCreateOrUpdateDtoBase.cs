using System;

namespace SuperAbp.Exam.Admin.ExamManagement.Exams;

public class ExamCreateOrUpdateDtoBase
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Score { get; set; }
    public decimal PassingScore { get; set; }
    public int TotalTime { get; set; }
    public Guid PaperId { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }

    public bool Published { get; set; }
    public bool RandomOrderOfOption { get; set; }

    /// <summary>
    /// 最大考试次数
    /// </summary>
    public int MaxNumberOfTimes { get; set; }

    /// <summary>
    /// 答题模式 <see cref="SuperAbp.Exam.ExamManagement.Exams.AnswerMode"/>
    /// </summary>
    public int AnswerMode { get; set; }

    /// <summary>
    /// 审核模式 <see cref="SuperAbp.Exam.ExamManagement.Exams.ReviewMode"/>
    /// </summary>
    public int ReviewMode { get; set; }
}