using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.MistakeReviews;

public class MistakeReviewListDto : AuditedEntityDto<Guid>
{
    public Guid QuestionId { get; set; }

    public required string QuestionContent { get; set; }
    public int ErrorCount { get; set; }
    public required int QuestionType { get; set; }
}