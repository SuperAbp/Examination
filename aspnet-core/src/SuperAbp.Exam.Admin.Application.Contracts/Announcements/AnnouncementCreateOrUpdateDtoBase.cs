using System;

namespace SuperAbp.Exam.Admin.Announcements;

public class AnnouncementCreateOrUpdateDtoBase
{
    public bool Publish { get; set; }
    public string Title { get; set; }

    public string Content { get; set; }

    public DateTime? PublishTime { get; set; }

    public DateTime? ExpirationTime { get; set; }

    public int Sort { get; set; }

    public Guid? CategoryId { get; set; }
}