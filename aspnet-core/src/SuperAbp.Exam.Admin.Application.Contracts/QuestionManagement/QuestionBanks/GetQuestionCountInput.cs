using System;

namespace SuperAbp.Exam.Admin.QuestionManagement.QuestionBanks;

/// <summary>
/// 获取题库题数量输入参数
/// </summary>
public class GetQuestionCountInput
{
    public Guid? QuestionBankId { get; set; }

    /// <summary>
    /// 题型，可选
    /// </summary>
    public int? QuestionType { get; set; }

    /// <summary>
    /// 知识点Id，可选
    /// </summary>
    public Guid? KnowledgePointId { get; set; }
}