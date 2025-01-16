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
    { Title = String.Empty; }

    [SetsRequiredMembers]
    protected internal QuestionRepo(Guid id, string title) : base(id)
    {
        Title = title;
    }

    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; internal set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
}