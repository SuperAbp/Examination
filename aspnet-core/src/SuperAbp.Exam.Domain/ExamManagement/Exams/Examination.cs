using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using SuperAbp.Exam.ExamManagement.Exams.States;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;

namespace SuperAbp.Exam.ExamManagement.Exams;

/// <summary>
/// 考试
/// </summary>
public class Examination : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    private IExaminationState _state;

    protected Examination()
    {
        Name = String.Empty;
        _state = new DraftState();
    }

    [SetsRequiredMembers]
    public Examination(Guid id, Guid paperId, string name, decimal score,
        decimal passingScore, int totalTime, AnswerMode answerMode, bool randomOrderOfOption,
        bool manualReview, ReviewMode reviewMode) : base(id)
    {
        Name = name;
        Score = score;
        PassingScore = passingScore;
        ManualReview = manualReview;
        TotalTime = totalTime;
        PaperId = paperId;
        _state = new DraftState();
        AnswerMode = answerMode;
        RandomOrderOfOption = randomOrderOfOption;
        ReviewMode = reviewMode;
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
    /// 人工批阅
    /// </summary>
    public bool ManualReview { get; set; }

    /// <summary>
    /// 试卷Id
    /// </summary>
    public Guid PaperId { get; set; }

    public ExaminationStatus Status
    {
        get => _state.Status;
        set
        {
            // 用于ORM设置状态
            _state = ExaminationStateFactory.GetState(value);
        }
    }

    public AnswerMode AnswerMode { get; set; }

    /// <summary>
    /// 最大考试次数
    /// </summary>
    public int MaxNumberOfTimes { get; set; }

    /// <summary>
    /// 选项乱序
    /// </summary>
    public bool RandomOrderOfOption { get; set; }

    /// <summary>
    /// 审核模式
    /// </summary>
    public ReviewMode ReviewMode { get; set; }

    /// <summary>
    /// 开始时间
    /// </summary>
    public DateTime? StartTime { get; private set; }

    /// <summary>
    /// 结束时间
    /// </summary>
    public DateTime? EndTime { get; private set; }

    public Guid? TenantId { get; set; }

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

    public void setEndTime(DateTime endTime)
    {
        EndTime = endTime;
    }

    /// <summary>
    /// 设置状态（由状态对象调用）
    /// </summary>
    internal void SetState(IExaminationState state)
    {
        _state = state;
    }

    /// <summary>
    /// 发布考试
    /// </summary>
    public void Publish()
    {
        _state.Publish(this);
    }

    /// <summary>
    /// 取消考试
    /// </summary>
    public void Cancel()
    {
        _state.Cancel(this);
    }

    /// <summary>
    /// 终止考试（提前结束）
    /// </summary>
    public void Terminate(DateTime endTime)
    {
        _state.Terminate(this, endTime);
    }

    /// <summary>
    /// 完成考试（评分完成）
    /// </summary>
    public void Complete()
    {
        _state.Complete(this);
    }

    /// <summary>
    /// 作废考试
    /// </summary>
    public void Invalidate()
    {
        _state.Invalidate(this);
    }

    /// <summary>
    /// 检查是否可以更新
    /// </summary>
    public bool CanUpdate()
    {
        return _state.CanUpdate();
    }
}