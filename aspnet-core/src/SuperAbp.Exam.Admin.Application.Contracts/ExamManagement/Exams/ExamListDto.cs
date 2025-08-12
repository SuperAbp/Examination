using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.ExamManagement.Exams
{
    /// <summary>
    /// 列表
    /// </summary>
    public class ExamListDto : EntityDto<Guid>
    {
        public required string Name { get; set; }
        public decimal Score { get; set; }
        public decimal PassingScore { get; set; }
        public int TotalTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public int Status { get; set; }
        public bool RandomOrderOfOption { get; set; }
        public int AnswerMode { get; set; }
    }
}