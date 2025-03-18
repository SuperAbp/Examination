using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.ExamManagement.UserExams
{
    /// <summary>
    /// 查询条件
    /// </summary>
    public class GetUserExamsInput: PagedAndSortedResultRequestDto
    {
        public Guid? ExamId { get; set; }
    }
}