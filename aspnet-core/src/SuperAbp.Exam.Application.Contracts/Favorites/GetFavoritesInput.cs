using SuperAbp.Exam.QuestionManagement.Questions;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Favorites;

public class GetFavoritesInput : PagedAndSortedResultRequestDto
{
    public QuestionType? QuestionType { get; set; }
    public string? QuestionName { get; set; }
}