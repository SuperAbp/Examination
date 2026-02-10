namespace SuperAbp.Exam.Admin.Announcements;

public class AnnouncementCategoryCreateOrUpdateDtoBase
{
    public string Name { get; set; }

    public int Sort { get; set; }

    public string? Remark { get; set; }
}
