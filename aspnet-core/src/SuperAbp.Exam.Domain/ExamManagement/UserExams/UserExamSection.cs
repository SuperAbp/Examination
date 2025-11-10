using SuperAbp.Exam.ExamManagement.UserExamQuestions;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace SuperAbp.Exam.ExamManagement.UserExams;

/// <summary>
/// 用户考试的试卷大题
/// </summary>
public class UserExamSection : Entity<Guid>, IMultiTenant
{
    protected UserExamSection()
    {
    }

    public UserExamSection(Guid id, Guid userExamId, Guid sectionId, string title, decimal scoreEach, decimal totalScore, int order, int totalCount) : base(id)
    {
        UserExamId = userExamId;
        SectionId = sectionId;
        Title = title;
        ScoreEach = scoreEach;
        TotalScore = totalScore;
        Order = order;
        TotalCount = totalCount;
        Questions = [];
    }

    public Guid UserExamId { get; set; }

    /// <summary>
    /// 对应的试卷大题ID
    /// </summary>
    public Guid SectionId { get; set; }

    public string Title { get; set; }

    public decimal ScoreEach { get; set; }

    public decimal TotalScore { get; set; }

    public int Order { get; set; }

    public int TotalCount { get; set; }

    public Guid? TenantId { get; set; }

    /// <summary>
    /// 该章节下的考试题目
    /// </summary>
    public List<UserExamQuestion> Questions { get; set; }

    public UserExamSection AddQuestion(UserExamQuestion question)
    {
        Questions.Add(question);
        return this;
    }

    public UserExamSection SetQuestions(List<UserExamQuestion> questions)
    {
        Questions = questions;
        return this;
    }
}
