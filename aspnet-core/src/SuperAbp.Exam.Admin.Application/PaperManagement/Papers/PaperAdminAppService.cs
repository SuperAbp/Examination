using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using SuperAbp.Exam.Permissions;
using SuperAbp.Exam.PaperManagement.PaperRepos;
using SuperAbp.Exam.PaperManagement.Papers;

namespace SuperAbp.Exam.Admin.PaperManagement.Papers
{
    [Authorize(ExamPermissions.Papers.Default)]
    public class PaperAdminAppService(IPaperRepository paperRepository, IPaperRepoRepository paperRepoRepository)
        : ExamAppService, IPaperAdminAppService
    {
        public virtual async Task<PagedResultDto<PaperListDto>> GetListAsync(GetPapersInput input)
        {
            await NormalizeMaxResultCountAsync(input);

            var queryable = await paperRepository.GetQueryableAsync();

            queryable = queryable.WhereIf(!input.Name.IsNullOrWhiteSpace(), e => e.Name.Contains(input.Name));

            long totalCount = await AsyncExecuter.CountAsync(queryable);

            var entities = await AsyncExecuter.ToListAsync(queryable
                .OrderBy(input.Sorting ?? PaperConsts.DefaultSorting)
                .PageBy(input));

            var dtos = ObjectMapper.Map<List<Paper>, List<PaperListDto>>(entities);

            return new PagedResultDto<PaperListDto>(totalCount, dtos);
        }

        public virtual async Task<GetPaperForEditorOutput> GetEditorAsync(Guid id)
        {
            Paper entity = await paperRepository.GetAsync(id);

            return ObjectMapper.Map<Paper, GetPaperForEditorOutput>(entity);
        }

        [Authorize(ExamPermissions.Papers.Create)]
        public virtual async Task<PaperListDto> CreateAsync(PaperCreateDto input)
        {
            if (await paperRepository.ExistsByNameAsync(input.Name.Trim()))
            {
                throw new UserFriendlyException(L["ExamExists"]);
            }

            Paper paper = new Paper(GuidGenerator.Create(), input.Name, input.Score)
            {
                Description = input.Description
            };

            paper = await paperRepository.InsertAsync(paper, true);
            return ObjectMapper.Map<Paper, PaperListDto>(paper);
        }

        [Authorize(ExamPermissions.Papers.Update)]
        public virtual async Task<PaperListDto> UpdateAsync(Guid id, PaperUpdateDto input)
        {
            Paper paper = await paperRepository.GetAsync(id);
            paper.Name = input.Name;
            paper.Score = input.Score;
            paper.Description = input.Description;
            paper = await paperRepository.UpdateAsync(paper);
            return ObjectMapper.Map<Paper, PaperListDto>(paper);
        }

        [Authorize(ExamPermissions.Papers.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await paperRepoRepository.DeleteByExamIdAsync(id);
            await paperRepository.DeleteAsync(id);
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