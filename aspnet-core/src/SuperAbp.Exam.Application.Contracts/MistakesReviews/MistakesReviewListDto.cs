using System;
using SuperAbp.Exam.QuestionManagement.Questions;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.MistakesReviews;

public class MistakesReviewListDto : AuditedEntityDto<Guid>
{
    public Guid QuestionId { get; set; }

    public required string QuestionContent { get; set; }
    public int ErrorCount { get; set; }
    public required QuestionType QuestionType { get; set; }
}