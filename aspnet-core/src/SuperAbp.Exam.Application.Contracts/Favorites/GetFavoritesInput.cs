using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Favorites;

public class GetFavoritesInput : PagedAndSortedResultRequestDto
{
    public int? QuestionType { get; set; }
    public string? QuestionContent { get; set; }
}