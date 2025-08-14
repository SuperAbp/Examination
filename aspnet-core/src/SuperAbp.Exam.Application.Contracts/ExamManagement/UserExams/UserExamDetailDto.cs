using SuperAbp.Exam.ExamManagement.Exams;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.ExamManagement.UserExams
{
    /// <summary>
    /// 详情
    /// </summary>
    public class UserExamDetailDto : EntityDto<Guid>
    {
        public Guid UserId { get; set; }
        public Guid ExamId { get; set; }
        public int Status { get; set; }

        public DateTime EndTime { get; set; }

        public required AnswerMode AnswerMode { get; set; }

        public IReadOnlyList<QuestionDto> Questions { get; set; } = [];

        public class QuestionDto
        {
            public Guid Id { get; set; }

            /// <summary>
            /// 题干
            /// </summary>
            public required string Content { get; set; }

            public int QuestionType { get; set; }

            /// <summary>
            /// 解析
            /// </summary>
            public string? Analysis { get; set; }

            public string? Answers { get; set; }

            /// <summary>
            /// 正确
            /// </summary>
            public bool? Right { get; set; }

            /// <summary>
            /// 得分
            /// </summary>
            public decimal? Score { get; set; }

            public decimal? QuestionScore { get; set; }

            public IReadOnlyList<string> KnowledgePoints { get; set; } = [];

            public List<OptionDto> Options { get; set; } = [];

            public class OptionDto
            {
                public Guid Id { get; set; }

                /// <summary>
                /// 答案
                /// </summary>
                public required string Content { get; set; }

                public bool? Right { get; set; }
            }
        }
    }
}