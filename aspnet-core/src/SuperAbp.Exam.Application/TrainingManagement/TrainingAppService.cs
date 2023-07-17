using SuperAbp.Exam.QuestionManagement.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SuperAbp.Exam.QuestionManagement.QuestionRepos;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;

namespace SuperAbp.Exam.TrainingManagement;

[Authorize]
public class TrainingAppService : ExamAppService, ITrainingAppService
{
    private readonly ITrainingRepository _trainingRepository;
    private readonly IQuestionRepoRepository _questionRepoRepository;
    private readonly IQuestionRepository _questionRepository;

    public TrainingAppService(ITrainingRepository trainingRepository, IQuestionRepoRepository questionRepoRepository, IQuestionRepository questionRepository)
    {
        _trainingRepository = trainingRepository;
        _questionRepoRepository = questionRepoRepository;
        _questionRepository = questionRepository;
    }

    public async Task<ListResultDto<TrainingListDto>> GetListAsync(GetTrainsInput input)
    {
        var trains = await _trainingRepository
            .GetListAsync(t => t.QuestionRepositoryId == input.QuestionRepositoryId
                               && t.UserId == CurrentUser.GetId());

        var dtos = ObjectMapper.Map<List<Training>, List<TrainingListDto>>(trains);

        return new ListResultDto<TrainingListDto>(dtos);
    }

    public async Task<TrainingListDto> CreateAsync(TrainingCreateDto input)
    {
        if (!await _questionRepoRepository.AnyAsync(input.QuestionRepositoryId))
        {
            throw new UserFriendlyException("题库不存在");
        }
        if (!await _questionRepository.AnyAsync(input.QuestionRepositoryId, input.QuestionId))
        {
            throw new UserFriendlyException("题目不存在");
        }

        if (await _trainingRepository.AnyQuestionAsync(input.QuestionId))
        {
            throw new UserFriendlyException("请勿重复答题");
        }
        var training = new Training(GuidGenerator.Create(), CurrentUser.GetId(), input.QuestionRepositoryId, input.QuestionId, input.Right);
        await _trainingRepository.InsertAsync(training);
        return ObjectMapper.Map<Training, TrainingListDto>(training);
    }

    public async Task SetIsRightAsync(Guid id, bool right)
    {
        var training = await _trainingRepository.GetAsync(id);
        training.Right = right;
        await _trainingRepository.UpdateAsync(training);
    }
}