using System;

namespace SuperAbp.Exam.ExamManagement.Exams
{
    /// <summary>
    /// 考试排名
    /// </summary>
    public class ExamRankingDto
    {
        /// <summary>
        /// 用户考试Id
        /// </summary>
        public Guid UserExamId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 总分
        /// </summary>
        public decimal TotalScore { get; set; }

        /// <summary>
        /// 是否通过
        /// </summary>
        public bool? IsPassed { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? FinishedTime { get; set; }

        /// <summary>
        /// 排名
        /// </summary>
        public int Rank { get; set; }
    }
}
