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
using SuperAbp.Exam.QuestionManagement.QuestionRepos;
using SuperAbp.Exam.QuestionManagement.Questions;

namespace SuperAbp.Exam.Admin.QuestionManagement.QuestionRepos
{
    /// <summary>
    /// 题库管理
    /// </summary>
    [Authorize(ExamPermissions.QuestionRepositories.Default)]
    public class QuestionRepoAppService : ExamAppService, IQuestionRepoAppService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IQuestionRepoRepository _questionRepoRepository;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="questionRepoRepository"></param>
        public QuestionRepoAppService(
            IQuestionRepoRepository questionRepoRepository, IQuestionRepository questionRepository)
        {
            _questionRepoRepository = questionRepoRepository;
            _questionRepository = questionRepository;
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public virtual async Task<QuestionRepoDetailDto> GetAsync(Guid id)
        {
            QuestionRepo entity = await _questionRepoRepository.GetAsync(id);

            return ObjectMapper.Map<QuestionRepo, QuestionRepoDetailDto>(entity);
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns>结果</returns>
        public virtual async Task<PagedResultDto<QuestionRepoListDto>> GetListAsync(GetQuestionReposInput input)
        {
            await NormalizeMaxResultCountAsync(input);

            var queryable = await _questionRepoRepository.GetQueryableAsync();

            queryable = queryable
                .WhereIf(!input.Title.IsNullOrWhiteSpace(), r => r.Title.Contains(input.Title));

            long totalCount = await AsyncExecuter.CountAsync(queryable);

            var entities = await AsyncExecuter.ToListAsync(queryable
                .OrderBy(input.Sorting ?? "Id DESC")
                .PageBy(input));
            var dtos = new List<QuestionRepoListDto>(); //ObjectMapper.Map<List<QuestionRepo>, List<QuestionRepoListDto>>(entities);
            foreach (var item in entities)
            {
                var dto = ObjectMapper.Map<QuestionRepo, QuestionRepoListDto>(item);
                dto.SingleCount = await _questionRepository.GetCountAsync(item.Id, QuestionType.SingleSelect);
                dto.JudgeCount = await _questionRepository.GetCountAsync(item.Id, QuestionType.Judge);
                dto.MultiCount = await _questionRepository.GetCountAsync(item.Id, QuestionType.MultiSelect);
                dtos.Add(dto);
            }
            return new PagedResultDto<QuestionRepoListDto>(totalCount, dtos);
        }

        /// <summary>
        /// 获取修改
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public virtual async Task<GetQuestionRepoForEditorOutput> GetEditorAsync(Guid id)
        {
            QuestionRepo entity = await _questionRepoRepository.GetAsync(id);

            return ObjectMapper.Map<QuestionRepo, GetQuestionRepoForEditorOutput>(entity);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(ExamPermissions.QuestionRepositories.Create)]
        public virtual async Task<QuestionRepoListDto> CreateAsync(QuestionRepoCreateDto input)
        {
            var entity = ObjectMapper.Map<QuestionRepoCreateDto, QuestionRepo>(input);
            entity = await _questionRepoRepository.InsertAsync(entity, true);
            return ObjectMapper.Map<QuestionRepo, QuestionRepoListDto>(entity);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(ExamPermissions.QuestionRepositories.Update)]
        public virtual async Task<QuestionRepoListDto> UpdateAsync(Guid id, QuestionRepoUpdateDto input)
        {
            QuestionRepo entity = await _questionRepoRepository.GetAsync(id);
            entity = ObjectMapper.Map(input, entity);
            entity = await _questionRepoRepository.UpdateAsync(entity);
            return ObjectMapper.Map<QuestionRepo, QuestionRepoListDto>(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [Authorize(ExamPermissions.QuestionRepositories.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _questionRepoRepository.DeleteAsync(s => s.Id == id);
        }

        /// <summary>
        /// 规范最大记录数
        /// </summary>
        /// <param name="input">参数</param>
        /// <returns></returns>
        private async Task NormalizeMaxResultCountAsync(PagedAndSortedResultRequestDto input)
        {
            var maxPageSize = (await SettingProvider.GetOrNullAsync(QuestionRepoSettings.MaxPageSize))?.To<int>();
            if (maxPageSize.HasValue && input.MaxResultCount > maxPageSize.Value)
            {
                input.MaxResultCount = maxPageSize.Value;
            }
        }
    }
}