using System;

namespace SuperAbp.Exam.Admin.ExamManagement.ExamRepos
{
    /// <summary>
    /// 创建
    /// </summary>
    public class ExamingRepoCreateDto : ExamingRepoCreateOrUpdateDtoBase
    {
        public Guid ExamingId { get; set; }
        public Guid QuestionRepositoryId { get; set; }
    }
}