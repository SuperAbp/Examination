using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SuperAbp.Exam.Admin.QuestionManagement.Questions;

/// <summary>
/// 创建
/// </summary>
public class QuestionCreateDto : QuestionCreateOrUpdateDtoBase
{
    [Required]
    public int QuestionType { get; set; }

    public List<QuestionCreateOrUpdateAnswerDto> Options { get; set; } = [];
}