using SuperAbp.Exam.QuestionManagement.QuestionAnswers;
using System;
using System.ComponentModel.DataAnnotations;

namespace SuperAbp.Exam.Admin.QuestionManagement.Questions;

public class QuestionCreateOrUpdateAnswerDto
{
    public Guid? Id { get; set; }

    [Required]
    public bool Right { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    [Required]
    [MaxLength(QuestionAnswerConsts.MaxContentLength)]
    public string Content { get; set; }

    /// <summary>
    /// 解析
    /// </summary>
    [MaxLength(QuestionAnswerConsts.MaxAnalysisLength)]
    public string? Analysis { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [Required]
    public int Sort { get; set; }
}