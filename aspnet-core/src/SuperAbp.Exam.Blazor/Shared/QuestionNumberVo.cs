using System;
using System.Collections.Generic;

namespace SuperAbp.Exam.Blazor.Shared;

public class QuestionNumberVo
{
    public string SectionTitle { get; set; }
    public decimal? TotalScore { get; set; }
    public IReadOnlyList<QuestionIndex> Questions { get; set; }

    public class QuestionIndex
    {
        public Guid QuestionId { get; set; }
        public decimal? Score { get; set; }
    }
}