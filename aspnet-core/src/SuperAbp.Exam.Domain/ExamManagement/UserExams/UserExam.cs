using SuperAbp.Exam.ExamManagement.UserExamQuestions;
using SuperAbp.Exam.ExamManagement.UserExams.States;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;
using static SuperAbp.Exam.ExamDomainErrorCodes;

namespace SuperAbp.Exam.ExamManagement.UserExams;

/// <summary>
/// 用户考试
/// </summary>
public class UserExam : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    private IUserExamState _state;

    protected UserExam()
    {
        _state = new WaitingState();
    }

    public UserExam(Guid id, Guid examId, Guid userId, bool isActive = true) : base(id)
    {
        UserId = userId;
        ExamId = examId;
        IsActive = isActive;
        _state = new WaitingState();
        Sections = new List<UserExamSection>();
    }

    public Guid UserId { get; protected set; }
    public Guid ExamId { get; protected set; }

    /// <summary>
    /// 总分
    /// </summary>
    public decimal TotalScore { get; set; }

    /// <summary>
    /// 是否通过
    /// </summary>
    public bool? IsPassed { get; set; }

    /// <summary>
    /// 交卷时间
    /// </summary>
    public DateTime? FinishedTime { get; set; }

    public DateTime? StartTime { get; set; }

    public UserExamStatus Status
    {
        get => _state.Status;
        set
        {
            // 用于ORM设置状态
            _state = UserExamStateFactory.GetState(value);
        }
    }

    public Guid? TenantId { get; set; }

    /// <summary>
    /// 是否为最新有效提交
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// 考试的试卷大题
    /// </summary>
    public List<UserExamSection> Sections { get; set; }

    public void ReviewQuestion(Guid reviewId, Guid questionId, bool right, decimal score, string? comment)
    {
        UserExamQuestion q = Sections
            .SelectMany(s => s.Questions)
            .FirstOrDefault(x => x.QuestionId == questionId) ?? throw new EntityNotFoundException("题目不存在");

        q.Review(reviewId, right, score, comment);
    }

    public void AnswerQuestion(Guid questionId, string answers)
    {
        if (!_state.CanAnswer())
        {
            throw new InvalidUserExamStatusException(Status);
        }
        UserExamQuestion q = Sections
            .SelectMany(s => s.Questions)
            .FirstOrDefault(x => x.QuestionId == questionId) ?? throw new EntityNotFoundException("题目不存在");
        q.Answers = answers;
    }

    public void UpdateTotalScore()
    {
        TotalScore = Sections
            .SelectMany(s => s.Questions)
            .Sum(q => q.Score ?? 0);
    }

    public bool IsSubmitted()
    {
        return new[]
        {
            UserExamStatus.Submitted,
            UserExamStatus.Scored
        }.Contains(Status);
    }

    public UserExam AddSection(UserExamSection section)
    {
        Sections.Add(section);
        return this;
    }

    /// <summary>
    /// 检查并设置是否通过考试
    /// </summary>
    /// <param name="passingScore">及格分数</param>
    public void CheckPassed(decimal passingScore)
    {
        IsPassed = TotalScore >= passingScore;
    }

    /// <summary>
    /// 设置状态（由状态对象调用）
    /// </summary>
    internal void SetState(IUserExamState state)
    {
        _state = state;
    }

    /// <summary>
    /// 开始考试
    /// </summary>
    public void Start(DateTime startTime)
    {
        _state.Start(this, startTime);
    }

    /// <summary>
    /// 提交考试
    /// </summary>
    public void Submit(bool requireManualReview)
    {
        _state.Submit(this, requireManualReview);
    }

    /// <summary>
    /// 批阅出分
    /// </summary>
    public void Score()
    {
        _state.Score(this);
    }

    /// <summary>
    /// 标记为超时
    /// </summary>
    public void Timeout()
    {
        _state.Timeout(this);
    }

    /// <summary>
    /// 标记为无效
    /// </summary>
    public void Invalidate()
    {
        _state.Invalidate(this);
    }
}