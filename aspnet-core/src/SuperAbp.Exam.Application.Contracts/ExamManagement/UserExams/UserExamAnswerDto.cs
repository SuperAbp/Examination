using System;

namespace SuperAbp.Exam.ExamManagement.UserExams
{
    /// <summary>
    /// 更新
    /// </summary>
    public class UserExamAnswerDto
    {
        public Guid QuestionId { get; set; }

        public required string Answers { get; set; }
    }
}