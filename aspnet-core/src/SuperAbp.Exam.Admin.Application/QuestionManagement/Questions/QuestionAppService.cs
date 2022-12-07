using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Linq;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using SuperAbp.Exam.QuestionManagement.Questions;
using SuperAbp.Exam.Permissions;
using SuperAbp.Exam.Admin.QuestionManagement.Questions;

namespace SuperAbp.Exam.Admin.QuestionManagement.Questions
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
            await NormalizeMaxResultCountAsync(input);

            var queryable = await _questionRepository.GetQueryableAsync();

            queryable = queryable
                .Where(q => q.QuestionRepositoryId == input.QuestionRepositoryId)
                .WhereIf(input.QuestionType.HasValue, q => q.QuestionType == input.QuestionType.Value)
                .WhereIf(!input.Content.IsNullOrWhiteSpace(), q => q.Content.Contains(input.Content));

            long totalCount = await AsyncExecuter.CountAsync(queryable);

            var entities = await AsyncExecuter.ToListAsync(queryable
                .OrderBy(input.Sorting ?? QuestionConsts.DefaultSorting)
                .PageBy(input));

            var dtos = ObjectMapper.Map<List<Question>, List<QuestionListDto>>(entities);

            return new PagedResultDto<QuestionListDto>(totalCount, dtos);
        }

        /// <summary>
        /// 获取修改
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public virtual async Task<GetQuestionForEditorOutput> GetEditorAsync(Guid id)
        {
            Question entity = await _questionRepository.GetAsync(id);

            return ObjectMapper.Map<Question, GetQuestionForEditorOutput>(entity);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(ExamPermissions.Questions.Create)]
        public virtual async Task<QuestionListDto> CreateAsync(QuestionCreateDto input)
        {
            var entity = ObjectMapper.Map<QuestionCreateDto, Question>(input);
            entity = await _questionRepository.InsertAsync(entity, true);
            return ObjectMapper.Map<Question, QuestionListDto>(entity);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(ExamPermissions.Questions.Update)]
        public virtual async Task<QuestionListDto> UpdateAsync(Guid id, QuestionUpdateDto input)
        {
            Question entity = await _questionRepository.GetAsync(id);
            entity = ObjectMapper.Map(input, entity);
            entity = await _questionRepository.UpdateAsync(entity);
            return ObjectMapper.Map<Question, QuestionListDto>(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [Authorize(ExamPermissions.Questions.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _questionRepository.DeleteAsync(s => s.Id == id);
        }

        /// <summary>
        /// 规范最大记录数
        /// </summary>
        /// <param name="input">参数</param>
        /// <returns></returns>
        private async Task NormalizeMaxResultCountAsync(PagedAndSortedResultRequestDto input)
        {
            var maxPageSize = (await SettingProvider.GetOrNullAsync(QuestionSettings.MaxPageSize))?.To<int>();
            if (maxPageSize.HasValue && input.MaxResultCount > maxPageSize.Value)
            {
                input.MaxResultCount = maxPageSize.Value;
            }
        }
    }
}