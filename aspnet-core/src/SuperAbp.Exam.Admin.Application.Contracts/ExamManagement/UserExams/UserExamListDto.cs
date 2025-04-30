using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.ExamManagement.UserExams
{
    /// <summary>
    /// 列表
    /// </summary>
    public class UserExamListDto : EntityDto<Guid>
    {
        public Guid ExamId { get; set; }

        /// <summary>
        /// 考试名称
        /// </summary>
        public string ExamName { get; set; }

        /// <summary>
        /// 最高分
        /// </summary>
        public decimal TotalScore { get; set; }

        /// <summary>
        /// 是否交卷
        /// </summary>
        public bool Finished { get; set; }

        public DateTime? FinishedTime { get; set; }

        public DateTime CreationTime { get; set; }
    }
}