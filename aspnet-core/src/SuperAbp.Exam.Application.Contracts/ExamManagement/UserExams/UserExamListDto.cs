using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.ExamManagement.UserExams
{
    /// <summary>
    /// 列表
    /// </summary>
    public class UserExamListDto: EntityDto<Guid>
    {
        /// <summary>
        /// 考试
        /// </summary>
        public string Exam { get; set; }
        public decimal TotalScore { get; set; }
    }
}