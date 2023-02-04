using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.ExamManagement.ExamRepos
{
    /// <summary>
    /// 查询条件
    /// </summary>
    public class GetExamingReposInput : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 考试Id
        /// </summary>
        public Guid ExamingId { get; set; }
    }
}