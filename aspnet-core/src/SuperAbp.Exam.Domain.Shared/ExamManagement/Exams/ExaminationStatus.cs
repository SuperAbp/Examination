using Ardalis.SmartEnum;
using SuperAbp.Exam.ExamManagement.UserExams;

namespace SuperAbp.Exam.ExamManagement.Exams;

/// <summary>
/// 考试状态
/// </summary>
public class ExaminationStatus : SmartEnum<ExaminationStatus>
{
    public static readonly ExaminationStatus Draft = new(nameof(Draft), 0);
    public static readonly ExaminationStatus Ongoing = new(nameof(Ongoing), 1);
    public static readonly ExaminationStatus Grading = new(nameof(Grading), 2);
    public static readonly ExaminationStatus Completed = new(nameof(Grading), 3);
    public static readonly ExaminationStatus Cancelled = new(nameof(Cancelled), 4);

    public ExaminationStatus(string name, int value) : base(name, value)
    {
    }
}