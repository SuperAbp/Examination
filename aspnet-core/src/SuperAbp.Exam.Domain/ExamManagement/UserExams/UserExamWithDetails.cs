using System;

namespace SuperAbp.Exam.ExamManagement.UserExams
{
    public class UserExamWithDetails
    {
        public Guid Id { get; set; }
        public Guid ExamId { get; set; }

        public string ExamName { get; set; }

        /// <summary>
        /// 总分
        /// </summary>
        public decimal TotalScore { get; set; }

        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 是否交卷
        /// </summary>
        public bool Finished { get; set; }

        public DateTime? FinishedTime { get; set; }
    }
}