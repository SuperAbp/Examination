using System;

namespace SuperAbp.Exam.Admin.QuestionManagement.QuestionAnswers
{
    /// <summary>
    /// 创建
    /// </summary>
    public class QuestionAnswerCreateDto : QuestionAnswerCreateOrUpdateDtoBase
    {
        public Guid QuestionId { get; set; }
    }
}