using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.QuestionManagement.Questions;

public class QuestionDetailDto : EntityDto<Guid>
{
    /// <summary>
    /// 题干
    /// </summary>
    public required string Content { get; set; }

    public string? Analysis { get; set; }

    public int QuestionType { get; set; }
    public Guid QuestionBankId { get; set; }

    public List<QuestionAnswerDto> Answers { get; set; } = [];
}