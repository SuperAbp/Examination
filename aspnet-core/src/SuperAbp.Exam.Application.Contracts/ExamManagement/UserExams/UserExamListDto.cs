using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.ExamManagement.UserExams
{
    /// <summary>
    /// 列表
    /// </summary>
    public class UserExamListDto: EntityDto<Guid>
    {
        public System.Guid UserId { get; set; }
        public System.Guid ExamId { get; set; }
        public decimal TotalScore { get; set; }
    }
}