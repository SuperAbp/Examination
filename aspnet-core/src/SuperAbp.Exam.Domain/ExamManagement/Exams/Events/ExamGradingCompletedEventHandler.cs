using SuperAbp.Exam.ExamManagement.UserExams;
using SuperAbp.Exam.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;
using Volo.Abp.Timing;

namespace SuperAbp.Exam.ExamManagement.Exams.Events;

/// <summary>
/// 考试评分完成事件处理器 - 通知所有参加考试的用户
/// </summary>
public class ExamGradingCompletedEventHandler : ILocalEventHandler<ExamGradingCompletedEvent>, ITransientDependency
{
    private readonly NotificationManager _notificationManager;
    private readonly IUserExamRepository _userExamRepository;
    private readonly IClock _clock;

    public ExamGradingCompletedEventHandler(
        NotificationManager notificationManager,
        IUserExamRepository userExamRepository,
        IClock clock)
    {
        _notificationManager = notificationManager;
        _userExamRepository = userExamRepository;
        _clock = clock;
    }

    public async Task HandleEventAsync(ExamGradingCompletedEvent eventData)
    {
        var userExams = await _userExamRepository.GetListAsync(examId: eventData.ExamId);
        var scoredUserExams = userExams
            .Where(ue => ue.IsActive && ue.Status == UserExamStatus.Scored)
            .ToList();

        if (!scoredUserExams.Any())
        {
            return;
        }

        var userIds = scoredUserExams.Select(ue => ue.UserId).Distinct().ToList();

        await _notificationManager.NotifyAsync(
               type: NotificationType.ExamScorePublished,
               receiverIds: userIds,
               data: new
               {
                   ExamName = eventData.ExamName,
                   CompletedTime = _clock.Now
               },
               relatedEntityId: eventData.ExamId,
               relatedEntityType: "Exam"
           );
    }
}
