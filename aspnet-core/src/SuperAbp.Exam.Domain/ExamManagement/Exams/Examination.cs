using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Timing;

namespace SuperAbp.Exam.ExamManagement.Exams;

/// <summary>
/// 考试
/// </summary>
public class Examination : FullAuditedAggregateRoot<Guid>
{
    protected Examination()
    { Name = String.Empty; }

    [SetsRequiredMembers]
    public Examination(Guid id, Guid paperId, string name, decimal score, decimal passingScore, int totalTime, AnswerMode answerMode, bool randomOrderOfOption) : base(id)
    {
        Name = name;
        Score = score;
        PassingScore = passingScore;
        TotalTime = totalTime;
        PaperId = paperId;
        Status = ExaminationStatus.Draft;
        AnswerMode = answerMode;
        RandomOrderOfOption = randomOrderOfOption;
    }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 分数
    /// </summary>
    public decimal Score { get; set; }

    /// <summary>
    /// 及格分
    /// </summary>
    public decimal PassingScore { get; set; }

    /// <summary>
    /// 时长
    /// </summary>
    public int TotalTime { get; set; }

    /// <summary>
    /// 试卷Id
    /// </summary>
    public Guid PaperId { get; set; }

    public ExaminationStatus Status { get; set; }

    public AnswerMode AnswerMode { get; set; }

    /// <summary>
    /// 选项乱序
    /// </summary>
    public bool RandomOrderOfOption { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? StartTime { get; private set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndTime { get; private set; }

    public void SetTime(DateTime? startTime, DateTime? endTime)
    {
        if (startTime.HasValue && endTime.HasValue)
        {
            if (endTime < startTime)
            {
                throw new UserFriendlyException("结束时间必须晚于开始时间！");
            }

            StartTime = startTime;
            EndTime = endTime;
        }
        else if (startTime.HasValue)
        {
            StartTime = startTime;
        }
        else if (endTime.HasValue)
        {
            EndTime = endTime;
        }
    }
}