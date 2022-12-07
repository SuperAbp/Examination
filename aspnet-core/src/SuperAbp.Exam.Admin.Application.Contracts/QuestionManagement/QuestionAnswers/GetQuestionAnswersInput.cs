using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.QuestionManagement.QuestionAnswers
{
    /// <summary>
    /// 查询条件
    /// </summary>
    public class GetQuestionAnswersInput : PagedAndSortedResultRequestDto
    {
        public Guid QuestionId { get; set; }
    }
}