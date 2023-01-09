using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace SuperAbp.Exam.QuestionManagement.QuestionAnswers;

/// <summary>
/// 答案
/// </summary>
public class QuestionAnswer : FullAuditedEntity<Guid>
{
    public QuestionAnswer()
    {
    }

    public QuestionAnswer(Guid id) : base(id)
    {
    }

    /// <summary>
    /// 是否正确
    /// </summary>
    public bool Right { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// 解析
    /// </summary>
    public string Analysis { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    public Guid QuestionId { get; set; }
}