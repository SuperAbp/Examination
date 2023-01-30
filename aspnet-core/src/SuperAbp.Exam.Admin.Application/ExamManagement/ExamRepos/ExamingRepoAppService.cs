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
using SuperAbp.Exam.ExamManagement.ExamRepos;

namespace SuperAbp.Exam.Admin.ExamManagement.ExamRepos
{
    /// <summary>
    /// 考试题库管理
    /// </summary>
    [Authorize(ExamPermissions.ExamRepos.Default)]
    public class ExamingRepoAppService : ExamAppService, IExamingRepoAppService
    {
        private readonly IExamingRepoRepository _examRepoRepository;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="examRepoRepository"></param>
        public ExamingRepoAppService(
            IExamingRepoRepository examRepoRepository)
        {
            _examRepoRepository = examRepoRepository;
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns>结果</returns>
        public virtual async Task<PagedResultDto<ExamingRepoListDto>> GetListAsync(GetExamingReposInput input)
        {
            await NormalizeMaxResultCountAsync(input);

            var queryable = await _examRepoRepository.GetQueryableAsync();

            long totalCount = await AsyncExecuter.CountAsync(queryable);

            var entities = await AsyncExecuter.ToListAsync(queryable
                .OrderBy(input.Sorting ?? ExamingRepoConsts.DefaultSorting)
                .PageBy(input));

            var dtos = ObjectMapper.Map<List<ExamingRepo>, List<ExamingRepoListDto>>(entities);

            return new PagedResultDto<ExamingRepoListDto>(totalCount, dtos);
        }

        /// <summary>
        /// 获取修改
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public virtual async Task<GetExamingRepoForEditorOutput> GetEditorAsync(Guid examingId, Guid questionRepositoryId)
        {
            ExamingRepo entity = await _examRepoRepository.GetAsync(examingId, questionRepositoryId);

            return ObjectMapper.Map<ExamingRepo, GetExamingRepoForEditorOutput>(entity);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(ExamPermissions.ExamRepos.Create)]
        public virtual async Task<ExamingRepoListDto> CreateAsync(ExamingRepoCreateDto input)
        {
            var entity = ObjectMapper.Map<ExamingRepoCreateDto, ExamingRepo>(input);
            entity = await _examRepoRepository.InsertAsync(entity, true);
            return ObjectMapper.Map<ExamingRepo, ExamingRepoListDto>(entity);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(ExamPermissions.ExamRepos.Update)]
        public virtual async Task<ExamingRepoListDto> UpdateAsync(Guid examingId, Guid questionRepositoryId, ExamingRepoUpdateDto input)
        {
            ExamingRepo entity = await _examRepoRepository.GetAsync(examingId, questionRepositoryId);
            entity = ObjectMapper.Map(input, entity);
            entity = await _examRepoRepository.UpdateAsync(entity);
            return ObjectMapper.Map<ExamingRepo, ExamingRepoListDto>(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [Authorize(ExamPermissions.ExamRepos.Delete)]
        public virtual async Task DeleteAsync(Guid examingId, Guid questionRepositoryId)
        {
            await _examRepoRepository.DeleteAsync(examingId, questionRepositoryId);
        }

        /// <summary>
        /// 规范最大记录数
        /// </summary>
        /// <param name="input">参数</param>
        /// <returns></returns>
        private async Task NormalizeMaxResultCountAsync(PagedAndSortedResultRequestDto input)
        {
            var maxPageSize = (await SettingProvider.GetOrNullAsync(ExamingRepoSettings.MaxPageSize))?.To<int>();
            if (maxPageSize.HasValue && input.MaxResultCount > maxPageSize.Value)
            {
                input.MaxResultCount = maxPageSize.Value;
            }
        }
    }
}