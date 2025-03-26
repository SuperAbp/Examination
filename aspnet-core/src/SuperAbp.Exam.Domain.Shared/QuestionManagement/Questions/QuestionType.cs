using Ardalis.SmartEnum;

namespace SuperAbp.Exam.QuestionManagement.Questions;

/// <summary>
/// 问题类型
/// </summary>
public class QuestionType : SmartEnum<QuestionType>
{
    public static readonly QuestionType SingleSelect = new QuestionType(nameof(SingleSelect), 0);
    public static readonly QuestionType MultiSelect = new QuestionType(nameof(MultiSelect), 1);
    public static readonly QuestionType Judge = new QuestionType(nameof(Judge), 2);
    public static readonly QuestionType FillInTheBlanks = new QuestionType(nameof(FillInTheBlanks), 3);

    public QuestionType(string name, int value) : base(name, value)
    {
    }
}