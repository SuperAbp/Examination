using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace SuperAbp.Exam.ExamManagement.ExamRepos
{
    /// <summary>
    /// 考试题库
    /// </summary>
    public class ExamingRepo : Entity<Guid>, IHasCreationTime,ISoftDelete
    {
        /// <summary>
        /// 考试Id
        /// </summary>
        public Guid ExamingId { get; set; }

        /// <summary>
        /// 题库Id
        /// </summary>
        public Guid QuestionRepositoryId { get; set; }

        /// <summary>
        /// 比例
        /// </summary>
        public decimal? Proportion { get; set; }

        /// <summary>
        /// 单选数量
        /// </summary>
        public int? SingleCount { get; set; }

        /// <summary>
        /// 单选分数
        /// </summary>
        public decimal? SingleScore { get; set; }

        /// <summary>
        /// 多选数量
        /// </summary>
        public int? MultiCount { get; set; }

        /// <summary>
        /// 多选分数
        /// </summary>
        public decimal? MultiScore { get; set; }

        /// <summary>
        /// 判断数量
        /// </summary>
        public int? JudgeCount { get; set; }

        /// <summary>
        /// 判断分数
        /// </summary>
        public decimal? JudgeScore { get; set; }

        public DateTime CreationTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}