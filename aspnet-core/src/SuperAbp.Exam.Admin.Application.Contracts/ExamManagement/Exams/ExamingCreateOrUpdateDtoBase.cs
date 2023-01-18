using System;

namespace SuperAbp.Exam.Admin.ExamManagement.Exams
{
    public class ExamingCreateOrUpdateDtoBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Score { get; set; }
        public decimal PassingScore { get; set; }
        public int TotalTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}