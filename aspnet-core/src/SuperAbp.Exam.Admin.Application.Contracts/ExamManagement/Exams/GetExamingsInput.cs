using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.ExamManagement.Exams
{
    /// <summary>
    /// 查询条件
    /// </summary>
    public class GetExamingsInput : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}