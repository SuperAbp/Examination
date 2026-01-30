using System;

namespace SuperAbp.Exam.KnowledgePoints;

/// <summary>
/// 知识点已删除事件
/// </summary>
public class KnowledgePointDeletedEvent
{
    public Guid KnowledgePointId { get; set; }
    public Guid? TenantId { get; set; }
}
