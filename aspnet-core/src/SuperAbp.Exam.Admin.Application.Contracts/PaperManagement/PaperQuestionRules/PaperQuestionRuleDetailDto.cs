using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.PaperManagement.PaperQuestionRules
{
    /// <summary>
    /// 列表
    /// </summary>
    public class PaperQuestionRuleDetailDto : EntityDto<Guid>
    {
        public int? RadioCount { get; set; }
        public decimal? RadioScore { get; set; }
        public int? MultiCount { get; set; }
        public decimal? MultiScore { get; set; }
        public int? JudgeCount { get; set; }
        public decimal? JudgeScore { get; set; }
    }
}