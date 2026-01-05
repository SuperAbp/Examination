using Ardalis.SmartEnum;

namespace SuperAbp.Exam.ExamManagement.Exams;

/// <summary>
/// 考试状态
/// </summary>
public class ExaminationStatus : SmartEnum<ExaminationStatus>
{
    /// <summary>
    /// 草稿
    /// </summary>
    public static readonly ExaminationStatus Draft = new(nameof(Draft), 0);

    /// <summary>
    /// 已发布
    /// </summary>
    public static readonly ExaminationStatus Published = new(nameof(Published), 1);

    /// <summary>
    /// 评分中
    /// </summary>
    public static readonly ExaminationStatus Grading = new(nameof(Grading), 2);

    /// <summary>
    /// 已完成
    /// </summary>
    public static readonly ExaminationStatus Completed = new(nameof(Completed), 3);

    /// <summary>
    /// 已取消
    /// </summary>
    public static readonly ExaminationStatus Cancelled = new(nameof(Cancelled), 4);

    /// <summary>
    /// 已作废
    /// </summary>
    public static readonly ExaminationStatus Invalidated = new(nameof(Invalidated), 99);

    public ExaminationStatus(string name, int value) : base(name, value)
    {
    }
}