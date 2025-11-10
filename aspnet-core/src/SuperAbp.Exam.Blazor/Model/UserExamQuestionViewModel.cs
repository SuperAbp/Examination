using SuperAbp.Exam.QuestionManagement.Questions;
using System;
using System.Collections.Generic;

namespace SuperAbp.Exam.Blazor.Model;

public class UserExamSectionViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public decimal TotalScore { get; set; }
    public int Order { get; set; }
    public int TotalCount { get; set; }
    public IReadOnlyList<UserExamQuestionViewModel> Questions { get; set; } = [];

    public class UserExamQuestionViewModel
    {
        public Guid Id { get; set; }

        public int QuestionType { get; set; }

        /// <summary>
        /// 题干
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 正确
        /// </summary>
        public bool? Right { get; set; }

        /// <summary>
        /// 得分
        /// </summary>
        public decimal? Score { get; set; }

        /// <summary>
        /// 解析
        /// </summary>
        public string Analysis { get; set; }

        public IReadOnlyList<string> KnowledgePoints { get; set; } = [];
        public IReadOnlyList<UserExamQuestionAnswerViewModel> Options { get; set; } = [];

        public string Answers { get; set; }

        public class UserExamQuestionAnswerViewModel
        {
            public Guid Id { get; set; }

            /// <summary>
            /// 是否正确
            /// </summary>
            public bool Right { get; set; }

            /// <summary>
            /// 内容
            /// </summary>
            public required string Content { get; set; }

            /// <summary>
            /// 解析
            /// </summary>
            public string? Analysis { get; set; }

            /// <summary>
            /// 排序
            /// </summary>
            public int Sort { get; set; }
        }
    }
}