using System.Collections.Generic;

namespace SuperAbp.Exam.Admin.QuestionManagement.Questions;

/// <summary>
/// 修改输出
/// </summary>
public class GetQuestionForEditorOutput : QuestionCreateOrUpdateDtoBase
{
    public int QuestionType { get; set; }

    public List<QuestionAnswerDto> Answers { get; set; } = [];
}