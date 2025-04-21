using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.PaperManagement.PaperQuestionRules
{
    /// <summary>
    /// 查询条件
    /// </summary>
    public class GetPaperQuestionRulesInput : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 试卷Id
        /// </summary>
        public Guid PaperId { get; set; }
    }
}