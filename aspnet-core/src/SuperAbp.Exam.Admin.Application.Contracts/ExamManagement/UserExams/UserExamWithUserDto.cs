using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.ExamManagement.UserExams
{
    /// <summary>
    /// 列表
    /// </summary>
    public class UserExamWithUserDto
    {
        public Guid UserId { get; set; }
        public required string User { get; set; }

        public int TotalCount { get; set; }

        public decimal MaxScore { get; set; }
    }
}