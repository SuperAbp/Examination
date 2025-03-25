using System;

namespace SuperAbp.Exam.TrainingManagement;

public class GetTrainsInput
{
    /// <summary>
    /// 题库Id
    /// </summary>
    public Guid? QuestionRepositoryId { get; set; }

    public int? TrainingSource { get; set; }
}