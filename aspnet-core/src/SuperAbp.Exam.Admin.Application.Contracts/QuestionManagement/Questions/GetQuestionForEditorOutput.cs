using SuperAbp.Exam.QuestionManagement.Questions;

namespace SuperAbp.Exam.Admin.QuestionManagement.Questions;

/// <summary>
/// 修改输出
/// </summary>
public class GetQuestionForEditorOutput : QuestionCreateOrUpdateDtoBase
{
    public QuestionType QuestionType { get; set; }
}