using System;

namespace SuperAbp.Exam.ExamManagement.UserExams
{
    public class UserExamWithDetails
    {
        public string Exam { get; set; }

        /// <summary>
        /// 总分
        /// </summary>
        public decimal TotalScore { get; set; }

        public DateTime CreationTime { get; set; }
    }
}