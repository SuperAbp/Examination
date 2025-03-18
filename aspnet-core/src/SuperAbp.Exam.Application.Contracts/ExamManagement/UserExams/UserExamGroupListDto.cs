using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.ExamManagement.UserExams
{
    /// <summary>
    /// 列表
    /// </summary>
    public class UserExamGroupListDto: EntityDto<Guid>
    {
        public Guid ExamId { get; set; }

        /// <summary>
        /// 考试名称
        /// </summary>
        public string ExamName { get; set; }
        /// <summary>
        /// 次数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 最高分
        /// </summary>
        public decimal MaxScore { get; set; }

        public DateTime LastTime { get; set; }
    }
}