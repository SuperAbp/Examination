using System.Collections.Generic;

namespace SuperAbp.Exam.Admin.Apps;

public class AppDataListDto
{
    public bool Group { get; set; }
    public bool HideInBreadcrumb { get; set; }
    public string Key { get; set; }
    public string Text { get; set; }
    public string Icon { get; set; }
    public string Link { get; set; }
    public int Badge { get; set; }
    public List<AppDataListDto> Children { get; set; }
}