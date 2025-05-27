using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.ExamManagement.Exams
{
    /// <summary>
    /// 详情
    /// </summary>
    public class ExamDetailDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public decimal Score { get; set; }
        public decimal PassingScore { get; set; }
        public int TotalTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}