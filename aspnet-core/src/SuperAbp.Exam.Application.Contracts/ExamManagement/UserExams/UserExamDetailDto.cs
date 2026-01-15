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
        /// <summary>
        /// 总分
        /// </summary>
        public decimal TotalScore { get; set; }

        /// <summary>
        /// 是否通过
        /// </summary>
        public bool? IsPassed { get; set; }

        public Guid UserId { get; set; }
        public Guid ExamId { get; set; }
        public required string ExamName { get; set; }
        public int Status { get; set; }

        /// <summary>
        /// 是否为最新有效提交
        /// </summary>
        public bool IsActive { get; set; }

        public DateTime EndTime { get; set; }

        public required int AnswerMode { get; set; }

        public IReadOnlyList<SectionDto> Sections { get; set; } = [];

        public class SectionDto
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public decimal ScoreEach { get; set; }
            public decimal TotalScore { get; set; }
            public int Order { get; set; }
            public int TotalCount { get; set; }
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

                /// <summary>
                /// 固定顺序（仅填空题有效）
                /// </summary>
                public bool FixedOrder { get; set; }

                /// <summary>
                /// 填空题空格数量
                /// </summary>
                public int BlankOptionsCount { get; set; }

                /// <summary>
                /// 选择题提供
                /// </summary>
                public IReadOnlyList<OptionDto> Options { get; set; } = [];

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
}