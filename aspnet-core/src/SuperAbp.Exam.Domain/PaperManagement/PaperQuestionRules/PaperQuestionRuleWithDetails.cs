using System;

namespace SuperAbp.Exam.PaperManagement.PaperQuestionRules;

public class PaperQuestionRuleWithDetails
{
    public Guid Id { get; set; }

    public required string QuestionBank { get; set; }

    /// <summary>
    /// 题库Id
    /// </summary>
    public Guid QuestionBankId { get; set; }

    /// <summary>
    /// 单选数量
    /// </summary>
    public int? SingleCount { get; set; }

    /// <summary>
    /// 单选分数
    /// </summary>
    public decimal? SingleScore { get; set; }

    /// <summary>
    /// 多选数量
    /// </summary>
    public int? MultiCount { get; set; }

    /// <summary>
    /// 多选分数
    /// </summary>
    public decimal? MultiScore { get; set; }

    /// <summary>
    /// 判断数量
    /// </summary>
    public int? JudgeCount { get; set; }

    /// <summary>
    /// 判断分数
    /// </summary>
    public decimal? JudgeScore { get; set; }

    /// <summary>
    /// 填空数量
    /// </summary>
    public int? BlankCount { get; set; }

    /// <summary>
    /// 填空分数
    /// </summary>
    public decimal? BlankScore { get; set; }

    public DateTime CreationTime { get; set; }
}