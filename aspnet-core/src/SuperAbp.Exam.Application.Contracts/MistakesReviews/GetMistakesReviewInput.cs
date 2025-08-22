using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.MistakesReviews;

public class GetMistakesReviewInput : PagedAndSortedResultRequestDto
{
    public int? QuestionType { get; set; }

    public string? QuestionContent { get; set; }
}