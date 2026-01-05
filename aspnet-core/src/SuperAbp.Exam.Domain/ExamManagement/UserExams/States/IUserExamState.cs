using System;

namespace SuperAbp.Exam.ExamManagement.UserExams.States;

/// <summary>
/// 用户考试状态接口
/// </summary>
public interface IUserExamState
{
    /// <summary>
    /// 状态名称
    /// </summary>
    UserExamStatus Status { get; }

    /// <summary>
    /// 开始考试
    /// </summary>
    void Start(UserExam userExam, DateTime startTime);

    /// <summary>
    /// 提交考试
    /// </summary>
    void Submit(UserExam userExam, bool requireManualReview);

    /// <summary>
    /// 批阅考试
    /// </summary>
    void Score(UserExam userExam);

    /// <summary>
    /// 标记为超时
    /// </summary>
    void Timeout(UserExam userExam);

    /// <summary>
    /// 标记为无效
    /// </summary>
    void Invalidate(UserExam userExam);

    /// <summary>
    /// 检查是否可以答题
    /// </summary>
    bool CanAnswer();
}
