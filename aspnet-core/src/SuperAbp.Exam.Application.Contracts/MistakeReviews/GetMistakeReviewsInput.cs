using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.MistakeReviews;

public class GetMistakeReviewsInput : PagedAndSortedResultRequestDto
{
    public int? QuestionType { get; set; }

    public string? QuestionContent { get; set; }
}