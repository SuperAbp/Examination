using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.ExamManagement.UserExamQuestions
{
    /// <summary>
    /// 查询条件
    /// </summary>
    public class GetUserExamQuestionsInput : PagedAndSortedResultRequestDto
    {
        public Guid UserExamId { get; set; }
    }
}