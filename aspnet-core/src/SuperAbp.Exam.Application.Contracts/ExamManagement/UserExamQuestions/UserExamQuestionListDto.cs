using SuperAbp.Exam.QuestionManagement.Questions;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.ExamManagement.UserExamQuestions
{
    /// <summary>
    /// 列表
    /// </summary>
    public class UserExamQuestionListDto : EntityDto<Guid>
    {
        public string Question { get; set; }

        public Guid QuestionId { get; set; }
        public int QuestionType { get; set; }
        public decimal QuestionScore { get; set; }
        public string Answers { get; set; }

        public List<QuestionAnswerListDto> QuestionAnswers { get; set; }

        public class QuestionAnswerListDto
        {
            public Guid Id { get; set; }

            /// <summary>
            /// 答案
            /// </summary>
            public string Content { get; set; }
        }
    }
}