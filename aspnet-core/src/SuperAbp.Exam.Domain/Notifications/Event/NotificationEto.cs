using System;

namespace SuperAbp.Exam.Notifications.Event;

/// <summary>
/// 通知事件传输对象
/// </summary>
public class NotificationEto
{
    public Guid Id { get; set; }
    public Guid ReceiverId { get; set; }
    public int Type { get; set; }
    public string? Data { get; set; }
    public DateTime CreationTime { get; set; }
    public Guid? RelatedEntityId { get; set; }
    public string? RelatedEntityType { get; set; }
}