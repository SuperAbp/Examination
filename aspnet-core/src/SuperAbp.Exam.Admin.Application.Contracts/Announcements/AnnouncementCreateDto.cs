using System;

namespace SuperAbp.Exam.Admin.Announcements;

public class AnnouncementCreateOrUpdateDtoBase
{
    public string Title { get; set; }

    public string Content { get; set; }

    public DateTime? ExpirationTime { get; set; }

    public int Sort { get; set; }

    public Guid? CategoryId { get; set; }
}

public class AnnouncementCreateDto : AnnouncementCreateOrUpdateDtoBase
{
}