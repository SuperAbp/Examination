using Ardalis.SmartEnum;

namespace SuperAbp.Exam.ExamManagement.Exams;

/// <summary>
/// 审核模式
/// </summary>
public class ReviewMode : SmartEnum<ReviewMode>
{
    /// <summary>
    /// 统一审核 - 考试结束后统一批阅
    /// </summary>
    public static readonly ReviewMode Unified = new(nameof(Unified), 0);

    /// <summary>
    /// 实时审核 - 提交后立即批阅
    /// </summary>
    public static readonly ReviewMode RealTime = new(nameof(RealTime), 1);

    public ReviewMode(string name, int value) : base(name, value)
    {
    }
}
