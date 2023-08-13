using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.PaperManagement.Papers
{
    /// <summary>
    /// 查询条件
    /// </summary>
    public class GetPapersInput : PagedAndSortedResultRequestDto
    {
        public string Name { get; set; }
    }
}