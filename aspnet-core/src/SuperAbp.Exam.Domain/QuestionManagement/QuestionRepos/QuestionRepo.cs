using System;
using System.Diagnostics.CodeAnalysis;
using Volo.Abp.Domain.Entities.Auditing;

namespace SuperAbp.Exam.QuestionManagement.QuestionRepos;

/// <summary>
/// 题库
/// </summary>
public class QuestionRepo : FullAuditedAggregateRoot<Guid>
{
    protected QuestionRepo()
    { }

    [SetsRequiredMembers]
    public QuestionRepo(Guid id, string title) : base(id)
    {
        Title = title;
    }

    /// <summary>
    /// 标题
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
}