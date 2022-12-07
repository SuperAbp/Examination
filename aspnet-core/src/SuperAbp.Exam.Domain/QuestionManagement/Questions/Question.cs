using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace SuperAbp.Exam.QuestionManagement.Questions;

/// <summary>
/// 题目
/// </summary>
public class Question : FullAuditedAggregateRoot<Guid>
{
    public Question()
    {
    }

    public Question(Guid id) : base(id)
    {
    }

    public QuestionType QuestionType { get; set; }

    /// <summary>
    /// 题干
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// 解析
    /// </summary>
    public string Analysis { get; set; }

    /// <summary>
    /// 所属题库
    /// </summary>
    public Guid QuestionRepositoryId { get; set; }
}