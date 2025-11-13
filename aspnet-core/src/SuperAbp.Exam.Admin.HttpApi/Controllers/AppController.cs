using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SuperAbp.Exam.Admin.Apps;

namespace SuperAbp.Exam.Admin.Controllers;

/// <summary>
/// App
/// </summary>
[Route("api/app")]
public class AppController : ExamController
{
    private readonly IAppService _appService;
    private readonly IConfiguration _configuration;

    public AppController(IConfiguration configuration, IAppService appService)
    {
        _configuration = configuration;
        _appService = appService;
    }

    /// <summary>
    /// 基础数据
    /// </summary>
    /// <returns></returns>
    [HttpGet("data")]
    public async Task<IActionResult> GetDataAsync()
    {
        List<AppDataListDto> resultMenus = await _appService.GetDataAsync();
        string avatar = string.Empty;
        return Ok(new
        {
            App = new
            {
                Name = _configuration["App:ApplicationName"],
                Description = _configuration["App:ApplicationDescription"]
            },
            User = new
            {
                CurrentUser.Name,
                CurrentUser.Email,
                Avatar = avatar
            },
            Menu = resultMenus
        });
    }
}