using Ardalis.SmartEnum;

namespace SuperAbp.Exam.ExamManagement.UserExams;

/// <summary>
/// 状态
/// </summary>
public class UserExamStatus : SmartEnum<UserExamStatus>
{
    /// <summary>
    /// 等待中
    /// </summary>
    public static readonly UserExamStatus Waiting = new(nameof(Waiting), 0);

    /// <summary>
    /// 进行中
    /// </summary>
    public static readonly UserExamStatus InProgress = new(nameof(InProgress), 1);

    /// <summary>
    /// 已提交
    /// </summary>
    public static readonly UserExamStatus Submitted = new(nameof(Submitted), 2);

    /// <summary>
    /// 已出分
    /// </summary>
    public static readonly UserExamStatus Scored = new(nameof(Scored), 3);

    /// <summary>
    /// 超时
    /// </summary>
    public static readonly UserExamStatus Timeout = new(nameof(Timeout), 98);

    /// <summary>
    /// 无效
    /// </summary>
    public static readonly UserExamStatus Invalidated = new(nameof(Invalidated), 99);

    public UserExamStatus(string name, int value) : base(name, value)
    {
    }
}