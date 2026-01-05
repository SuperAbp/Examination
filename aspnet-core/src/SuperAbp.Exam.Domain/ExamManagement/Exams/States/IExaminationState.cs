using System;

namespace SuperAbp.Exam.ExamManagement.Exams.States;

/// <summary>
/// 考试状态接口
/// </summary>
public interface IExaminationState
{
    /// <summary>
    /// 状态名称
    /// </summary>
    ExaminationStatus Status { get; }

    /// <summary>
    /// 发布考试
    /// </summary>
    void Publish(Examination examination);

    /// <summary>
    /// 取消考试
    /// </summary>
    void Cancel(Examination examination);

    /// <summary>
    /// 终止考试（提前结束）
    /// </summary>
    void Terminate(Examination examination, DateTime endTime);

    /// <summary>
    /// 完成考试（评分完成）
    /// </summary>
    void Complete(Examination examination);

    /// <summary>
    /// 作废考试
    /// </summary>
    void Invalidate(Examination examination);

    /// <summary>
    /// 检查是否可以更新
    /// </summary>
    bool CanUpdate();
}
