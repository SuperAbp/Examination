using SuperAbp.Exam.QuestionManagement.Questions;
using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.QuestionManagement.Questions;

/// <summary>
/// 列表
/// </summary>
public class QuestionListDto : EntityDto<Guid>
{
    public QuestionType QuestionType { get; set; }
    public string Content { get; set; }
    public string Analysis { get; set; }
}