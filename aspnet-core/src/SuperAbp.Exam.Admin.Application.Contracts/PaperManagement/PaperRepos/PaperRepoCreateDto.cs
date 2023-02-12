using System;

namespace SuperAbp.Exam.Admin.PaperManagement.PaperRepos
{
    /// <summary>
    /// 创建
    /// </summary>
    public class PaperRepoCreateDto : PaperRepoCreateOrUpdateDtoBase
    {
        /// <summary>
        /// 考试Id
        /// </summary>
        public Guid ExamingId { get; set; }

        /// <summary>
        /// 题库Id
        /// </summary>
        public Guid QuestionRepositoryId { get; set; }
    }
}