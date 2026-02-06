using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.Announcements;

public class AnnouncementCategoryListDto : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; }

    public int Sort { get; set; }

    public string? Remark { get; set; }
}