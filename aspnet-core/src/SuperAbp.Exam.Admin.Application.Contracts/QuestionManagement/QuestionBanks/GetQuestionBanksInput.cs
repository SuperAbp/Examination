using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.QuestionManagement.QuestionBanks
{
    /// <summary>
    /// 查询条件
    /// </summary>
    public class GetQuestionBanksInput : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string? Title { get; set; }
    }
}