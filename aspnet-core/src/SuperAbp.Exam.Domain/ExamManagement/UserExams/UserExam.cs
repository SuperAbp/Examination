using System;
using System.Collections.Generic;
using System.Linq;
using SuperAbp.Exam.ExamManagement.UserExamQuestions;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using static SuperAbp.Exam.ExamDomainErrorCodes;

namespace SuperAbp.Exam.ExamManagement.UserExams;

/// <summary>
/// 用户考试
/// </summary>
public class UserExam : FullAuditedAggregateRoot<Guid>
{
    protected UserExam()
    {
    }

    public UserExam(Guid id, Guid examId, Guid userId) : base(id)
    {
        UserId = userId;
        ExamId = examId;
        Status = UserExamStatus.NotStarted;
    }

    public Guid UserId { get; protected set; }
    public Guid ExamId { get; protected set; }

    /// <summary>
    /// 总分
    /// </summary>
    public decimal TotalScore { get; set; }

    /// <summary>
    /// 交卷时间
    /// </summary>
    public DateTime? FinishedTime { get; set; }

    public UserExamStatus Status { get; set; }

    public ICollection<UserExamQuestion> Questions { get; set; }

    public void ReviewQuestion(Guid reviewId, Guid questionId, bool right, decimal score, string? comment)
    {
        UserExamQuestion q = Questions.FirstOrDefault(x => x.QuestionId == questionId) ?? throw new EntityNotFoundException("题目不存在");

        q.Review(reviewId, right, score, comment);
    }

    public void AnswerQuestion(Guid questionId, string answers)
    {
        if (Status != UserExamStatus.InProgress)
        {
            throw new InvalidUserExamStatusException(Status);
        }
        UserExamQuestion q = Questions.FirstOrDefault(x => x.QuestionId == questionId) ?? throw new EntityNotFoundException("题目不存在");
        q.Answers = answers;
    }

    public void UpdateTotalScore()
    {
        TotalScore = Questions.Sum(q => q.Score ?? 0);
    }

    public bool IsSubmitted()
    {
        return new[]
        {
            UserExamStatus.Submitted,
            UserExamStatus.Scored
        }.Contains(Status);
    }
}