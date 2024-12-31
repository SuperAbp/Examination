using System.Collections.Generic;

namespace SuperAbp.Exam.Admin.QuestionManagement.Questions;

public class QuestionImportModel
{
    public required string Title { get; set; }

    public List<Option> Options { get; set; } = [];

    public string? Analysis { get; set; } = null;

    public List<int> Answers { get; set; } = [];

    public class Option
    {
        /// <summary>
        /// 内容
        /// </summary>
        public required string Content { get; set; }

        /// <summary>
        /// 解析
        /// </summary>
        public string? Analysis { get; set; }
    }
}