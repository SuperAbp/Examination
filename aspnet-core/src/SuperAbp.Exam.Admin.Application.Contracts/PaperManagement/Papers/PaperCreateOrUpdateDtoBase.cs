using System;

namespace SuperAbp.Exam.Admin.PaperManagement.Papers;

public class PaperCreateOrUpdateDtoBase
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Score { get; set; }
    public int PaperType { get; set; }
    public PaperSectionDto[] Sections { get; set; } = [];

    public class PaperSectionDto
    {
        public Guid? Id { get; set; }
        public required string Title { get; set; }
        public decimal ScoreEach { get; set; }
        public decimal TotalScore { get; set; }
        public int Order { get; set; }
        public int TotalCount { get; set; }
        public string? Remark { get; set; }

        public PaperQuestionRuleDto[] PaperQuestionRules { get; set; } = [];
        public PaperQuestionDto[] PaperQuestions { get; set; } = [];

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