using Microsoft.AspNetCore.Mvc;
using SuperAbp.Exam.QuestionManagement.Questions;
using SuperAbp.Exam.TrainingManagement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Controllers;

[Route("api/training")]
public class TrainingController : ExamController, ITrainingAppService
{
    private readonly ITrainingAppService _trainingAppService;

    public TrainingController(ITrainingAppService trainingAppService)
    {
        _trainingAppService = trainingAppService;
    }

    [HttpPost]
    public async Task<TrainingListDto> CreateAsync(TrainingCreateDto input)
    {
        return await _trainingAppService.CreateAsync(input);
    }

    [HttpGet]
    public async Task<ListResultDto<TrainingListDto>> GetListAsync(GetTrainsInput input)
    {
        return await _trainingAppService.GetListAsync(input);
    }

    [HttpPatch]
    public async Task SetIsRightAsync(Guid id, bool right)
    {
        await _trainingAppService.SetIsRightAsync(id, right);
    }
}