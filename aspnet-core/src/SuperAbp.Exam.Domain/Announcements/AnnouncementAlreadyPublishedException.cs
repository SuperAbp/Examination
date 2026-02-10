using Volo.Abp;

namespace SuperAbp.Exam.Announcements;

public class AnnouncementAlreadyPublishedException : BusinessException
{
    public AnnouncementAlreadyPublishedException()
        : base(code: ExamDomainErrorCodes.Announcements.CannotUpdatePublished)
    {
    }
}
