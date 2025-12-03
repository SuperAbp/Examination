using SuperAbp.Exam.PaperManagement.Papers;
using SuperAbp.Exam.PaperManagement.PaperSections;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SuperAbp.Exam.Admin.PaperManagement.Papers;

public class PaperCreateOrUpdateDtoBase
{
    [Required]
    [MaxLength(PaperConsts.MaxNameLength)]
    public string Name { get; set; }

    [MaxLength(PaperConsts.MaxDescriptionLength)]
    public string? Description { get; set; }

    public decimal Score { get; set; }
    public int PaperType { get; set; }

    [MinLength(1)]
    public List<PaperSectionDto> Sections { get; set; } = [];

    public class PaperSectionDto
    {
        public Guid? Id { get; set; }

        [Required]
        [MaxLength(PaperSectionConsts.MaxTitleLength)]
        public string Title { get; set; }

        public decimal ScoreEach { get; set; }
        public decimal TotalScore { get; set; }
        public int Order { get; set; }
        public int TotalCount { get; set; }
        public string? Remark { get; set; }

        public List<PaperQuestionRuleDto> PaperQuestionRules { get; set; } = [];
        public List<PaperQuestionDto> PaperQuestions { get; set; } = [];

        public class PaperQuestionDto
        {
            public Guid QuestionId { get; set; }
            public decimal Score { get; set; }
            public int Order { get; set; }
        }

        public class PaperQuestionRuleDto
        {
            public Guid? Id { get; set; }
            public Guid QuestionBankId { get; set; }
            public int QuestionType { get; set; }
            public int Count { get; set; }
            public decimal Score { get; set; }
        }
    }
}