using System;
using System.Collections.Generic;

namespace SuperAbp.Exam.QuestionManagement.Questions
{
    public class QuestionWithDetails
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 题库
        /// </summary>
        public string QuestionBank { get; set; }

        public QuestionType QuestionType { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public List<string>? KnowledgePoints { get; set; }

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