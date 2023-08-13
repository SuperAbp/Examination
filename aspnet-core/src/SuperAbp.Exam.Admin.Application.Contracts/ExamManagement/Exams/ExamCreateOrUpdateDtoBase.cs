using System;

namespace SuperAbp.Exam.Admin.ExamManagement.Exams
{
    public class ExamCreateOrUpdateDtoBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Score { get; set; }
        public decimal PassingScore { get; set; }
        public int TotalTime { get; set; }
        public Guid PaperId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}