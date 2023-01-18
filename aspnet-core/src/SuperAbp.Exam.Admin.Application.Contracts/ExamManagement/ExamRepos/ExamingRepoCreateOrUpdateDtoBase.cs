using System;

namespace SuperAbp.Exam.Admin.ExamManagement.ExamRepos
{
    public class ExamingRepoCreateOrUpdateDtoBase
    {
        public Guid ExamingId { get; set; }
        public Guid QuestionRepositoryId { get; set; }
        public int? RadioCount { get; set; }
        public decimal? RadioScore { get; set; }
        public int? MultiCount { get; set; }
        public decimal? MultiScore { get; set; }
        public int? JudgeCount { get; set; }
        public decimal? JudgeScore { get; set; }
    }
}