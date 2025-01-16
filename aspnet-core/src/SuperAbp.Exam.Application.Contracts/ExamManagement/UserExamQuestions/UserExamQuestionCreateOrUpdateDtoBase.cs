using System;

namespace SuperAbp.Exam.ExamManagement.UserExamQuestions
{
    public class UserExamQuestionCreateOrUpdateDtoBase
    {
        public Guid UserExamId { get; set; }
        public Guid QuestionId { get; set; }
        public string? Answers { get; set; }
        public Boolean Right { get; set; }
    }
}