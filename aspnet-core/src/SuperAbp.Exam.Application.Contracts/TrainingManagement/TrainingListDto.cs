using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.TrainingManagement;

public class TrainingListDto : EntityDto<Guid>
{
    /// <summary>
    /// 题目Id
    /// </summary>
    public Guid QuestionId { get; set; }

    public bool Right { get; set; }
}