using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.QuestionManagement.QuestionRepos
{
    /// <summary>
    /// 查询条件
    /// </summary>
    public class GetQuestionReposInput : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string? Title { get; set; }
    }
}