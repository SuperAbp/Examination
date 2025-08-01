using Ardalis.SmartEnum;

namespace SuperAbp.Exam.ExamManagement.Exams;

/// <summary>
/// 答题模式
/// </summary>
public class AnswerMode(string name, int value) : SmartEnum<AnswerMode>(name, value)
{
    public static readonly AnswerMode All = new(nameof(All), 0);
    public static readonly AnswerMode Single = new(nameof(Single), 1);
}