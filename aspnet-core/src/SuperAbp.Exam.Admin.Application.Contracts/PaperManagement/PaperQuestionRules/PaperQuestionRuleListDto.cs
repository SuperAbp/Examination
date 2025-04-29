using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.PaperManagement.PaperQuestionRules
{
    /// <summary>
    /// 列表
    /// </summary>
    public class PaperQuestionRuleListDto : EntityDto<Guid>
    {
        public string QuestionBank { get; set; }

        /// <summary>
        /// 题库Id
        /// </summary>
        public Guid QuestionBankId { get; set; }

        public int? SingleCount { get; set; }
        public decimal? SingleScore { get; set; }
        public int? MultiCount { get; set; }
        public decimal? MultiScore { get; set; }
        public int? JudgeCount { get; set; }
        public decimal? JudgeScore { get; set; }
        public int? BlankCount { get; set; }
        public decimal? BlankScore { get; set; }
    }
}