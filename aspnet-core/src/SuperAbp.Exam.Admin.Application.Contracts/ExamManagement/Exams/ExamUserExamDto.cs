using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.ExamManagement.Exams
{
    /// <summary>
    /// 列表
    /// </summary>
    public class ExamUserExamDto
    {
        public Guid UserId { get; set; }
        public int Rank { get; set; }
        public required string User { get; set; }

        public int TotalCount { get; set; }

        public bool? IsPassed { get; set; }

        public decimal MaxScore { get; set; }
    }
}