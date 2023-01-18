using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.ExamManagement.Exams
{
    /// <summary>
    /// 列表
    /// </summary>
    public class ExamingListDto: EntityDto<Guid>
    {
        public string Name { get; set; }
        public decimal Score { get; set; }
        public decimal PassingScore { get; set; }
        public int TotalTime { get; set; }
        public System.Nullable<System.DateTime> StartTime { get; set; }
        public System.Nullable<System.DateTime> EndTime { get; set; }
    }
}