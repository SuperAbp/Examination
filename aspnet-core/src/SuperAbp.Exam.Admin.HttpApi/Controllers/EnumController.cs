using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SuperAbp.Exam.Admin.Enums;

namespace SuperAbp.Exam.Admin.Controllers;

/// <summary>
/// 枚举
/// </summary>
[Route("api/enum")]
public class EnumController:ExamController, IEnumAppService
{
    private  readonly IEnumAppService _enumAppService;

    public EnumController(IEnumAppService enumAppService)
    {
        _enumAppService = enumAppService;
    }

    /// <summary>
    /// 问题类型
    /// </summary>
    /// <returns></returns>
    [HttpGet("question-type")]
    public async Task<IDictionary<int, string>> GetQuestionTypeAsync()
    {
        return await _enumAppService.GetQuestionTypeAsync();
    }
}