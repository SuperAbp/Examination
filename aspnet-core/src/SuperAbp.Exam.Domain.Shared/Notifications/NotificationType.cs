using Ardalis.SmartEnum;

namespace SuperAbp.Exam.Notifications;

/// <summary>
/// 通知类型
/// </summary>
public class NotificationType : SmartEnum<NotificationType>
{
    public static readonly NotificationType ExamStartReminder = new(nameof(ExamStartReminder), 0);
    public static readonly NotificationType ExamEndReminder = new(nameof(ExamEndReminder), 1);
    public static readonly NotificationType ExamScorePublished = new(nameof(ExamScorePublished), 2);
    public static readonly NotificationType CommentReply = new(nameof(CommentReply), 3);
    public static readonly NotificationType TrainingUpdated = new(nameof(TrainingUpdated), 4);
    public static readonly NotificationType CertificateIssued = new(nameof(CertificateIssued), 5);
    public static readonly NotificationType QuestionApproved = new(nameof(QuestionApproved), 6);

    public NotificationType(string name, int value) : base(name, value)
    {
    }
}
