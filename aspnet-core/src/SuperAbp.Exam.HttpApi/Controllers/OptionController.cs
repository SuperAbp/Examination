using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SuperAbp.Exam.Options;

namespace SuperAbp.Exam.Controllers;

/// <summary>
/// 选项
/// </summary>
[Route("api/options")]
public class OptionController(IOptionAppService optionAppService) : ExamController, IOptionAppService
{
    [HttpGet("question-types")]
    public Dictionary<int, string> GetQuestionTypes()
    {
        return optionAppService.GetQuestionTypes();
    }
}