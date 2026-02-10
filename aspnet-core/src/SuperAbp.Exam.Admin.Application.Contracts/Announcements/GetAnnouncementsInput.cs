using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.Announcements;

public class GetAnnouncementsInput : PagedAndSortedResultRequestDto
{
    public string? Title { get; set; }

    public Guid? CategoryId { get; set; }

    public bool? IsPublished { get; set; }
}
