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
using SuperAbp.Exam.PaperManagement.PaperRepos;
using SuperAbp.Exam.PaperManagement.Papers;

namespace SuperAbp.Exam.Admin.PaperManagement.Papers
{
    /// <summary>
    /// 试卷管理
    /// </summary>
    [Authorize(ExamPermissions.Papers.Default)]
    public class PaperAppService : ExamAppService, IPaperAppService
    {
        private readonly IPaperRepository _examingRepository;
        private readonly IPaperRepoRepository _examingRepoRepository;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="examingRepository"></param>
        /// <param name="examingRepoRepository"></param>
        public PaperAppService(
            IPaperRepository examingRepository, IPaperRepoRepository examingRepoRepository)
        {
            _examingRepository = examingRepository;
            _examingRepoRepository = examingRepoRepository;
        }
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns>结果</returns>
        public virtual async Task<PagedResultDto<PaperListDto>> GetListAsync(GetPapersInput input)
        {
            await NormalizeMaxResultCountAsync(input);

            var queryable = await _examingRepository.GetQueryableAsync();

            long totalCount = await AsyncExecuter.CountAsync(queryable);

            var entities = await AsyncExecuter.ToListAsync(queryable
                .OrderBy(input.Sorting ?? "Id DESC")
                .PageBy(input));

            var dtos = ObjectMapper.Map<List<Paper>, List<PaperListDto>>(entities);

            return new PagedResultDto<PaperListDto>(totalCount, dtos);
        }

        /// <summary>
        /// 获取修改
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public virtual async Task<GetPaperForEditorOutput> GetEditorAsync(Guid id)
        {
            Paper entity = await _examingRepository.GetAsync(id);

            return ObjectMapper.Map<Paper, GetPaperForEditorOutput>(entity);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(ExamPermissions.Papers.Create)]
        public virtual async Task<PaperListDto> CreateAsync(PaperCreateDto input)
        {
            if (await _examingRepository.ExistsByNameAsync(input.Name.Trim()))
            {
                throw new UserFriendlyException(L["ExamingExists"]);
            }
            var entity = ObjectMapper.Map<PaperCreateDto, Paper>(input);
            entity = await _examingRepository.InsertAsync(entity, true);
            return ObjectMapper.Map<Paper, PaperListDto>(entity);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(ExamPermissions.Papers.Update)]
        public virtual async Task<PaperListDto> UpdateAsync(Guid id, PaperUpdateDto input)
        {
            var entity = await _examingRepository.GetAsync(id);
            entity = ObjectMapper.Map(input, entity);
            entity = await _examingRepository.UpdateAsync(entity);
            return ObjectMapper.Map<Paper, PaperListDto>(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [Authorize(ExamPermissions.Papers.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _examingRepoRepository.DeleteByExamingIdAsync(id);
            await _examingRepository.DeleteAsync(s => s.Id == id);
        }

        /// <summary>
        /// 规范最大记录数
        /// </summary>
        /// <param name="input">参数</param>
        /// <returns></returns>
        private async Task NormalizeMaxResultCountAsync(PagedAndSortedResultRequestDto input)
        {
            var maxPageSize = (await SettingProvider.GetOrNullAsync(PaperSettings.MaxPageSize))?.To<int>();
            if (maxPageSize.HasValue && input.MaxResultCount > maxPageSize.Value)
            {
                input.MaxResultCount = maxPageSize.Value;
            }
        }
    }
}