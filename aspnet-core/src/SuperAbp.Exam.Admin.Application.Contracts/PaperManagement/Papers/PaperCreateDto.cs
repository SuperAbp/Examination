using System;
using System.Collections.Generic;

namespace SuperAbp.Exam.Admin.PaperManagement.Papers
{
    /// <summary>
    /// 创建
    /// </summary>
    public class PaperCreateDto : PaperCreateOrUpdateDtoBase
    {
        public PaperCreatePaperRepoDto[] Repositories { get; set; } = [];

        public class PaperCreatePaperRepoDto
        {
            /// <summary>
            /// 题库Id
            /// </summary>
            public Guid QuestionRepositoryId { get; set; }

            public int? SingleCount { get; set; }
            public decimal? SingleScore { get; set; }
            public int? MultiCount { get; set; }
            public decimal? MultiScore { get; set; }
            public int? JudgeCount { get; set; }
            public decimal? JudgeScore { get; set; }

            /// <summary>
            /// 填空数量
            /// </summary>
            public int? BlankCount { get; set; }

            /// <summary>
            /// 填空分数
            /// </summary>
            public decimal? BlankScore { get; set; }
        }
    }
}