using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SuperAbp.Exam.Permissions;
using SuperAbp.Exam.QuestionManagement.QuestionRepos;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.QuestionManagement.Questions
{
    /// <summary>
    /// 问题管理
    /// </summary>
    [Authorize(ExamPermissions.Questions.Default)]
    public class QuestionAppService : ExamAppService, IQuestionAppService
    {
        private readonly IQuestionRepository _questionRepository;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="questionRepository"></param>
        public QuestionAppService(
            IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns>结果</returns>
        public virtual async Task<PagedResultDto<QuestionListDto>> GetListAsync(GetQuestionsInput input)
        {
            var queryable = await _questionRepository.GetQueryableAsync();

            queryable = queryable
                .Where(q => q.QuestionRepositoryId == input.QuestionRepositoryId);

            long totalCount = await AsyncExecuter.CountAsync(queryable);

            var entities = await AsyncExecuter.ToListAsync(queryable
                .OrderBy(QuestionConsts.DefaultSorting));

            var dtos = ObjectMapper.Map<List<Question>, List<QuestionListDto>>(entities);

            return new PagedResultDto<QuestionListDto>(totalCount, dtos);
        }

        public async Task<ListResultDto<Guid>> GetIdsAsync(GetQuestionsInput input)
        {
            var queryable = await _questionRepository.GetQueryableAsync();
            queryable = queryable
                .Where(q => q.QuestionRepositoryId == input.QuestionRepositoryId);
            var ids = await AsyncExecuter.ToListAsync(queryable.Select(q => q.Id));
            return new ListResultDto<Guid>(ids);
        }

        public virtual async Task<QuestionDetailDto> GetAsync(Guid id)
        {
            var entity = await _questionRepository.GetAsync(id);

            return ObjectMapper.Map<Question, QuestionDetailDto>(entity);
        }
    }
}