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
using SuperAbp.Exam.Permissions;
using SuperAbp.Exam.QuestionManagement.QuestionAnswers;

namespace SuperAbp.Exam.Admin.QuestionManagement.QuestionAnswers
{
    /// <summary>
    /// 答案管理
    /// </summary>
    [Authorize(ExamPermissions.QuestionAnswers.Default)]
    public class QuestionAnswerAppService : ExamAppService, IQuestionAnswerAppService
    {
        private readonly IQuestionAnswerRepository _questionAnswerRepository;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="questionAnswerRepository"></param>
        public QuestionAnswerAppService(
            IQuestionAnswerRepository questionAnswerRepository)
        {
            _questionAnswerRepository = questionAnswerRepository;
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns>结果</returns>
        public virtual async Task<PagedResultDto<QuestionAnswerListDto>> GetListAsync(GetQuestionAnswersInput input)
        {
            await NormalizeMaxResultCountAsync(input);

            var queryable = await _questionAnswerRepository.GetQueryableAsync();

            queryable = queryable.Where(a => a.QuestionId == input.QuestionId);

            long totalCount = await AsyncExecuter.CountAsync(queryable);

            var entities = await AsyncExecuter.ToListAsync(queryable
                .OrderBy(input.Sorting ?? QuestionAnswerConsts.DefaultSorting)
                .PageBy(input));

            var dtos = ObjectMapper.Map<List<QuestionAnswer>, List<QuestionAnswerListDto>>(entities);

            return new PagedResultDto<QuestionAnswerListDto>(totalCount, dtos);
        }

        /// <summary>
        /// 获取修改
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public virtual async Task<GetQuestionAnswerForEditorOutput> GetEditorAsync(Guid id)
        {
            QuestionAnswer entity = await _questionAnswerRepository.GetAsync(id);

            return ObjectMapper.Map<QuestionAnswer, GetQuestionAnswerForEditorOutput>(entity);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(ExamPermissions.QuestionAnswers.Create)]
        public virtual async Task<QuestionAnswerListDto> CreateAsync(QuestionAnswerCreateDto input)
        {
            var entity = ObjectMapper.Map<QuestionAnswerCreateDto, QuestionAnswer>(input);
            entity = await _questionAnswerRepository.InsertAsync(entity, true);
            return ObjectMapper.Map<QuestionAnswer, QuestionAnswerListDto>(entity);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(ExamPermissions.QuestionAnswers.Update)]
        public virtual async Task<QuestionAnswerListDto> UpdateAsync(Guid id, QuestionAnswerUpdateDto input)
        {
            QuestionAnswer entity = await _questionAnswerRepository.GetAsync(id);
            entity = ObjectMapper.Map(input, entity);
            entity = await _questionAnswerRepository.UpdateAsync(entity);
            return ObjectMapper.Map<QuestionAnswer, QuestionAnswerListDto>(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [Authorize(ExamPermissions.QuestionAnswers.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _questionAnswerRepository.DeleteAsync(s => s.Id == id);
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