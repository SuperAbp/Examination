using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.QuestionManagement.Questions;

/// <summary>
/// 列表
/// </summary>
public class QuestionListDto : EntityDto<Guid>
{
    public QuestionType QuestionType { get; private set; }
}