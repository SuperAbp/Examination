using System.Threading.Tasks;

namespace SuperAbp.Exam.Notifications;

/// <summary>
/// 通知发布器接口
/// </summary>
public interface INotificationPublisher
{
    /// <summary>
    /// 发布通知
    /// </summary>
    Task PublishAsync(Notification notification);
}
