using Ardalis.SmartEnum;

namespace SuperAbp.Exam.ExamManagement.Exams;

/// <summary>
/// 考试状态
/// </summary>
public class ExaminationStatus : SmartEnum<ExaminationStatus>
{
    public static readonly ExaminationStatus Draft = new(nameof(Draft), 0);
    public static readonly ExaminationStatus Published = new(nameof(Published), 1);
    public static readonly ExaminationStatus Grading = new(nameof(Grading), 2);
    public static readonly ExaminationStatus Completed = new(nameof(Completed), 3);
    public static readonly ExaminationStatus Cancelled = new(nameof(Cancelled), 4);

    public ExaminationStatus(string name, int value) : base(name, value)
    {
    }
}