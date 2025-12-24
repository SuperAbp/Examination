using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Mistakes;

public class GetMistakesInput : PagedAndSortedResultRequestDto
{
    public int? QuestionType { get; set; }

    public string? QuestionContent { get; set; }
}