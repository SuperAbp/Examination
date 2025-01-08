using SuperAbp.Exam.QuestionManagement.Questions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SuperAbp.Exam.QuestionManagement.QuestionRepos;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;

namespace SuperAbp.Exam.TrainingManagement;

[Authorize]
public class TrainingAppService(
    ITrainingRepository trainingRepository,
    IQuestionRepoRepository questionRepoRepository,
    IQuestionRepository questionRepository)
    : ExamAppService, ITrainingAppService
{
    public async Task<ListResultDto<TrainingListDto>> GetListAsync(GetTrainsInput input)
    {
        List<Training> trains = await trainingRepository
            .GetListAsync(t => t.QuestionRepositoryId == input.QuestionRepositoryId
                               && t.UserId == CurrentUser.GetId());

        List<TrainingListDto> dtos = ObjectMapper.Map<List<Training>, List<TrainingListDto>>(trains);

        return new ListResultDto<TrainingListDto>(dtos);
    }

    public async Task<TrainingListDto> CreateAsync(TrainingCreateDto input)
    {
        if (!await questionRepoRepository.IdExistsAsync(input.QuestionRepositoryId))
        {
            throw new UserFriendlyException("题库不存在");
        }
        if (!await questionRepository.AnyAsync(input.QuestionRepositoryId, input.QuestionId))
        {
            throw new UserFriendlyException("题目不存在");
        }

        if (await trainingRepository.AnyQuestionAsync(input.QuestionId))
        {
            throw new UserFriendlyException("请勿重复答题");
        }
        Training training = new(GuidGenerator.Create(), CurrentUser.GetId(), input.QuestionRepositoryId, input.QuestionId, input.Right);
        await trainingRepository.InsertAsync(training);
        return ObjectMapper.Map<Training, TrainingListDto>(training);
    }

    public async Task SetIsRightAsync(Guid id, bool right)
    {
        var training = await trainingRepository.GetAsync(id);
        training.Right = right;
        await trainingRepository.UpdateAsync(training);
    }
}