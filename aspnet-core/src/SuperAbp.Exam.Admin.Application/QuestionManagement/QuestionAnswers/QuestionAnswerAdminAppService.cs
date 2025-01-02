using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using SuperAbp.Exam.Permissions;
using SuperAbp.Exam.QuestionManagement.QuestionAnswers;

namespace SuperAbp.Exam.Admin.QuestionManagement.QuestionAnswers
{
    [Authorize(ExamPermissions.QuestionAnswers.Default)]
    public class QuestionAnswerAdminAppService(QuestionAnswerManager questionAnswerManager, IQuestionAnswerRepository questionAnswerRepository)
        : ExamAppService, IQuestionAnswerAdminAppService
    {
        public virtual async Task<PagedResultDto<QuestionAnswerListDto>> GetListAsync(GetQuestionAnswersInput input)
        {
            await NormalizeMaxResultCountAsync(input);

            var queryable = await questionAnswerRepository.GetQueryableAsync();

            queryable = queryable.Where(a => a.QuestionId == input.QuestionId);

            long totalCount = await AsyncExecuter.CountAsync(queryable);

            var entities = await AsyncExecuter.ToListAsync(queryable
                .OrderBy(input.Sorting ?? QuestionAnswerConsts.DefaultSorting)
                .PageBy(input));

            var dtos = ObjectMapper.Map<List<QuestionAnswer>, List<QuestionAnswerListDto>>(entities);

            return new PagedResultDto<QuestionAnswerListDto>(totalCount, dtos);
        }

        public virtual async Task<GetQuestionAnswerForEditorOutput> GetEditorAsync(Guid id)
        {
            QuestionAnswer entity = await questionAnswerRepository.GetAsync(id);

            return ObjectMapper.Map<QuestionAnswer, GetQuestionAnswerForEditorOutput>(entity);
        }

        [Authorize(ExamPermissions.QuestionAnswers.Create)]
        public virtual async Task<QuestionAnswerListDto> CreateAsync(QuestionAnswerCreateDto input)
        {
            QuestionAnswer answer = await questionAnswerManager.CreateAsync(input.QuestionId, input.Content, input.Right);
            answer.Analysis = input.Analysis;
            answer.Sort = input.Sort;

            answer = await questionAnswerRepository.InsertAsync(answer, true);
            return ObjectMapper.Map<QuestionAnswer, QuestionAnswerListDto>(answer);
        }

        [Authorize(ExamPermissions.QuestionAnswers.Update)]
        public virtual async Task<QuestionAnswerListDto> UpdateAsync(Guid id, QuestionAnswerUpdateDto input)
        {
            QuestionAnswer answer = await questionAnswerRepository.GetAsync(id);
            answer.Content = input.Content;
            answer.Analysis = input.Analysis;
            answer.Sort = input.Sort;
            answer.Right = input.Right;
            answer = await questionAnswerRepository.UpdateAsync(answer);
            return ObjectMapper.Map<QuestionAnswer, QuestionAnswerListDto>(answer);
        }

        [Authorize(ExamPermissions.QuestionAnswers.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await questionAnswerRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 规范最大记录数
        /// </summary>
        /// <param name="input">参数</param>
        /// <returns></returns>
        private async Task NormalizeMaxResultCountAsync(PagedAndSortedResultRequestDto input)
        {
            var maxPageSize = (await SettingProvider.GetOrNullAsync(QuestionAnswerSettings.MaxPageSize))?.To<int>();
            if (maxPageSize.HasValue && input.MaxResultCount > maxPageSize.Value)
            {
                input.MaxResultCount = maxPageSize.Value;
            }
        }
    }
}