using System;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace SuperAbp.Exam.PaperManagement.PaperQuestionRules
{
    /// <summary>
    /// 抽题规则
    /// </summary>
    public class PaperQuestionRule : Entity<Guid>, IHasCreationTime, ISoftDelete
    {
        protected PaperQuestionRule()
        { }

        public PaperQuestionRule(Guid id, Guid paperId, Guid questionBankId) : base(id)
        {
            PaperId = paperId;
            QuestionBankId = questionBankId;
        }

        /// <summary>
        /// 试卷Id
        /// </summary>
        public Guid PaperId { get; set; }

        /// <summary>
        /// 题库Id
        /// </summary>
        public Guid QuestionBankId { get; set; }

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

        /// <summary>
        /// 填空数量
        /// </summary>
        public int? BlankCount { get; set; }

        /// <summary>
        /// 填空分数
        /// </summary>
        public decimal? BlankScore { get; set; }

        public DateTime CreationTime { get; set; }

        public bool IsDeleted { get; set; }
    }
}