using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace SuperAbp.Exam.Notifications;

/// <summary>
/// 通知发布器接口
/// </summary>
public interface INotificationPublisher : ISingletonDependency
{
    /// <summary>
    /// 发布通知
    /// </summary>
    Task PublishAsync(Notification notification);
}