using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace SuperAbp.Exam.PaperManagement.Papers;

/// <summary>
/// 试卷
/// </summary>
public class Paper : FullAuditedAggregateRoot<Guid>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }
        
    /// <summary>
    /// 总题数
    /// </summary>
    public int TotalQuestionCount { get; set; }

    /// <summary>
    /// 分数
    /// </summary>
    public decimal Score { get; set; }

}