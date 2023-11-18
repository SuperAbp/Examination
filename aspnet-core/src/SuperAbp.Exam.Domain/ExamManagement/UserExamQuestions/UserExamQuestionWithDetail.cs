using System;
using System.Collections.Generic;
using SuperAbp.Exam.QuestionManagement.Questions;

namespace SuperAbp.Exam.ExamManagement.UserExamQuestions;

public class UserExamQuestionWithDetail
{
    public Guid Id { get; set; }

    public Guid QuestionId { get; set; }
    public QuestionType QuestionType { get; set; }

    /// <summary>
    /// 题干
    /// </summary>
    public string Question { get; set; }

    public decimal QuestionScore { get; set; }

    public string Answers { get; set; }

    public List<QuestionAnswer> QuestionAnswers { get; set; } = new List<QuestionAnswer>();

    public class QuestionAnswer
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 答案
        /// </summary>
        public string Content { get; set; }
    }
}