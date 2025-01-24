using SuperAbp.Exam.QuestionManagement.Questions;

namespace SuperAbp.Exam.Admin.QuestionManagement.Questions;

/// <summary>
/// 创建
/// </summary>
public class QuestionCreateDto : QuestionCreateOrUpdateDtoBase
{
    public QuestionType QuestionType { get; set; }
    public QuestionCreateOrUpdateAnswerDto[] Options { get; set; } = [];
}