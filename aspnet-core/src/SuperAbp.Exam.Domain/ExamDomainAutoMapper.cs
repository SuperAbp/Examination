using AutoMapper;
using SuperAbp.Exam.Notifications;

namespace SuperAbp.Exam;

public class ExamDomainAutoMapper : Profile
{
    public ExamDomainAutoMapper()
    {
        CreateMap<Notification, NotificationEto>();
    }
}