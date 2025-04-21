using System;

namespace SuperAbp.Exam.QuestionManagement.Questions
{
    public class QuestionBankWithDetails
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 题库
        /// </summary>
        public string QuestionBank { get; set; }

        public QuestionType QuestionType { get; set; }

        /// <summary>
        /// 题干
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 解析
        /// </summary>
        public string Analysis { get; set; }

        public DateTime CreationTime { get; set; }
    }
}