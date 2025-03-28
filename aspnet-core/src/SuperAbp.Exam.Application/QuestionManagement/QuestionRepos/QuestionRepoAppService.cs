using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SuperAbp.Exam.QuestionManagement.Questions;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.QuestionManagement.QuestionRepos
{
    public class QuestionRepoAppService(
        IQuestionRepoRepository questionRepoRepository,
        IQuestionRepository questionRepository)
        : ExamAppService, IQuestionRepoAppService
    {
        public virtual async Task<ListResultDto<QuestionType>> GetQuestionTypesAsync(Guid id)
        {
            List<QuestionType> questionTypes = await questionRepository.GetQuestionTypesAsync(id);
            return new ListResultDto<QuestionType>(questionTypes);
        }

        public virtual async Task<PagedResultDto<QuestionRepoListDto>> GetListAsync(GetQuestionReposInput input)
        {
            long totalCount = await questionRepoRepository.GetCountAsync(input.Title);

            List<QuestionRepo> entities = await questionRepoRepository
                .GetListAsync(input.Sorting ?? QuestionRepoConsts.DefaultSorting, input.SkipCount,
                    input.MaxResultCount, input.Title);

            List<QuestionRepoListDto> dtos = ObjectMapper.Map<List<QuestionRepo>, List<QuestionRepoListDto>>(entities);
            return new PagedResultDto<QuestionRepoListDto>(totalCount, dtos);
        }

        public async Task<QuestionRepoDetailDto> GetAsync(Guid id)
        {
            QuestionRepo questionRepo = await questionRepoRepository.GetAsync(id);

            QuestionRepoDetailDto dto = ObjectMapper.Map<QuestionRepo, QuestionRepoDetailDto>(questionRepo);
            return dto;
        }
    }
}