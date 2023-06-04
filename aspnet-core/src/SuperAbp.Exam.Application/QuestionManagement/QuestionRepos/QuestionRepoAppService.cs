using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SuperAbp.Exam.Permissions;
using SuperAbp.Exam.QuestionManagement.Questions;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.QuestionManagement.QuestionRepos
{
    /// <summary>
    /// 题库管理
    /// </summary>
    public class QuestionRepoAppService : ExamAppService, IQuestionRepoAppService
    {
        private readonly IQuestionRepoRepository _questionRepoRepository;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="questionRepoRepository"></param>
        public QuestionRepoAppService(
            IQuestionRepoRepository questionRepoRepository)
        {
            _questionRepoRepository = questionRepoRepository;
        }

        public virtual async Task<PagedResultDto<QuestionRepoListDto>> GetListAsync(GetQuestionReposInput input)
        {
            long totalCount = await _questionRepoRepository.GetCountAsync(input.Title);

            var entities = await _questionRepoRepository
                .GetListAsync(input.Title, input.Sorting ?? QuestionRepoConsts.DefaultSorting, input.SkipCount,
                    input.MaxResultCount);

            var dtos = ObjectMapper.Map<List<QuestionRepo>, List<QuestionRepoListDto>>(entities);
            return new PagedResultDto<QuestionRepoListDto>(totalCount, dtos);
        }

        public async Task<QuestionRepoDetailDto> GetAsync(Guid id)
        {
            var questionRepo = await _questionRepoRepository.GetAsync(id);

            var dto = ObjectMapper.Map<QuestionRepo, QuestionRepoDetailDto>(questionRepo);
            return dto;
        }
    }
}