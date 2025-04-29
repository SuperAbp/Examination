using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SuperAbp.Exam.Favorites;
using SuperAbp.Exam.KnowledgePoints;
using SuperAbp.Exam.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Users;

namespace SuperAbp.Exam.QuestionManagement.Questions
{
    [Authorize(ExamPermissions.Questions.Default)]
    public class QuestionAppService(IQuestionRepository questionRepository, IFavoriteRepository favoriteRepository, QuestionManager questionManager) : ExamAppService, IQuestionAppService
    {
        public IQuestionRepository QuestionRepository { get; } = questionRepository;
        public IFavoriteRepository FavoriteRepository { get; } = favoriteRepository;
        protected QuestionManager QuestionManager { get; } = questionManager;

        public virtual async Task<PagedResultDto<QuestionListDto>> GetListAsync(GetQuestionsInput input)
        {
            IQueryable<Question> queryable = await FilterAsync(input);

            long totalCount = await AsyncExecuter.CountAsync(queryable);

            List<Question> entities = await AsyncExecuter.ToListAsync(queryable
                .OrderBy(QuestionConsts.DefaultSorting));

            List<QuestionListDto> dtos = ObjectMapper.Map<List<Question>, List<QuestionListDto>>(entities);

            return new PagedResultDto<QuestionListDto>(totalCount, dtos);
        }

        public async Task<ListResultDto<Guid>> GetIdsAsync(GetQuestionsInput input)
        {
            IQueryable<Question> queryable = await FilterAsync(input);
            List<Guid> ids = await AsyncExecuter.ToListAsync(queryable.Select(q => q.Id));
            return new ListResultDto<Guid>(ids);
        }

        private async Task<IQueryable<Question>> FilterAsync(GetQuestionsInput input)
        {
            IQueryable<Question> queryable = await QuestionRepository.GetQueryableAsync();
            queryable = queryable
                .WhereIf(input.QuestionId.HasValue, q => q.Id == input.QuestionId.Value)
                .WhereIf(input.QuestionBankId.HasValue, q => q.QuestionBankId == input.QuestionBankId.Value)
                .WhereIf(!input.Content.IsNullOrWhiteSpace(), q => q.Content.Contains(input.Content))
                .WhereIf(input.QuestionType.HasValue, q => q.QuestionType == input.QuestionType.Value);
            if (input.IsFavorite)
            {
                var favoriteQueryable = await FavoriteRepository.GetQueryableAsync();
                queryable = from q in queryable
                            join f in favoriteQueryable on q.Id equals f.QuestionId
                            where f.CreatorId == CurrentUser.GetId()
                            select q;
            }
            return queryable;
        }

        public virtual async Task<QuestionDetailDto> GetAsync(Guid id)
        {
            Question entity = await QuestionRepository.GetAsync(id);
            List<KnowledgePoint> knowledgePoints = await QuestionManager.GetKnowledgePointsAsync(id);
            var dto = ObjectMapper.Map<Question, QuestionDetailDto>(entity);
            if (knowledgePoints.Count > 0)
            {
                dto.KnowledgePoints = knowledgePoints.Select(kp => kp.Name).ToArray();
            }

            return dto;
        }
    }
}