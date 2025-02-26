using System;

namespace SuperAbp.Exam.Admin.QuestionManagement.Questions;

/// <summary>
/// 更新
/// </summary>
public class QuestionUpdateDto : QuestionCreateOrUpdateDtoBase
{
    public QuestionCreateOrUpdateAnswerDto[] Options { get; set; } = [];
}