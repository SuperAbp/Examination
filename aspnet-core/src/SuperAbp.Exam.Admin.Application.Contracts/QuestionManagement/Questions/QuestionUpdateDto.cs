using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SuperAbp.Exam.Admin.QuestionManagement.Questions;

/// <summary>
/// 更新
/// </summary>
public class QuestionUpdateDto : QuestionCreateOrUpdateDtoBase
{
    [Required]
    [MinLength(1)]
    public List<QuestionCreateOrUpdateAnswerDto> Options { get; set; } = [];
}