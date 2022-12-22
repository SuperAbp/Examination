using SuperAbp.Exam.QuestionManagement.Questions;
using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.QuestionManagement.Questions;

/// <summary>
/// 列表
/// </summary>
public class QuestionListDto : EntityDto<Guid>
{
    /// <summary>
    /// 题库
    /// </summary>
    public string QuestionRepository { get; set; }

    public QuestionType QuestionType { get; set; }

    /// <summary>
    /// 题干
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// 解析
    /// </summary>
    public string Analysis { get; set; }
}