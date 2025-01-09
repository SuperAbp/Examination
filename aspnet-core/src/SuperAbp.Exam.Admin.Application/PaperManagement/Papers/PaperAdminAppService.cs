using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using SuperAbp.Exam.Permissions;
using SuperAbp.Exam.PaperManagement.PaperRepos;
using SuperAbp.Exam.PaperManagement.Papers;
using static SuperAbp.Exam.Admin.PaperManagement.Papers.PaperCreateDto;
using static SuperAbp.Exam.Admin.PaperManagement.Papers.PaperUpdateDto;

namespace SuperAbp.Exam.Admin.PaperManagement.Papers
{
    [Authorize(ExamPermissions.Papers.Default)]
    public class PaperAdminAppService(IPaperRepository paperRepository, IPaperRepoRepository paperRepoRepository, PaperManager paperManager)
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
            Paper paper = await paperManager.CreateAsync(input.Name, input.Score);
            paper.Description = input.Description;
            // TODO: Set total question count.
            paper.TotalQuestionCount = input.Repositories.Sum(p => p.SingleCount + p.MultiCount + p.JudgeCount + p.BlankCount) ?? 0;
            paper = await paperRepository.InsertAsync(paper);
            await CreatePaperRepoAsync(paper.Id, input.Repositories);
            return ObjectMapper.Map<Paper, PaperListDto>(paper);
        }

        protected virtual async Task CreatePaperRepoAsync(Guid paperId, PaperCreatePaperRepoDto[] dtos)
        {
            PaperRepo[] paperRepos = dtos.Select(input => new PaperRepo(GuidGenerator.Create(), paperId, input.QuestionRepositoryId)
            {
                BlankCount = input.BlankCount,
                BlankScore = input.BlankScore,
                SingleCount = input.SingleCount,
                SingleScore = input.SingleScore,
                MultiCount = input.MultiCount,
                MultiScore = input.MultiScore,
                JudgeCount = input.JudgeCount,
                JudgeScore = input.JudgeScore
            }).ToArray();
            await paperRepoRepository.InsertManyAsync(paperRepos);
        }

        [Authorize(ExamPermissions.Papers.Update)]
        public virtual async Task<PaperListDto> UpdateAsync(Guid id, PaperUpdateDto input)
        {
            Paper paper = await paperRepository.GetAsync(id);
            await paperManager.SetNameAsync(paper, input.Name);
            paper.Score = input.Score;
            paper.Description = input.Description;
            paper = await paperRepository.UpdateAsync(paper);
            await UpdatePaperRepoAsync(id, input.Repositories);
            return ObjectMapper.Map<Paper, PaperListDto>(paper);
        }

        protected virtual async Task UpdatePaperRepoAsync(Guid paperId, PaperUpdatePaperRepoDto[] dtos)
        {
            await paperRepoRepository.DeleteByPaperIdAsync(paperId);
            PaperRepo[] paperRepos = dtos.Select(input => new PaperRepo(GuidGenerator.Create(), paperId, input.QuestionRepositoryId)
            {
                BlankCount = input.BlankCount,
                BlankScore = input.BlankScore,
                SingleCount = input.SingleCount,
                SingleScore = input.SingleScore,
                MultiCount = input.MultiCount,
                MultiScore = input.MultiScore,
                JudgeCount = input.JudgeCount,
                JudgeScore = input.JudgeScore
            }).ToArray();
            await paperRepoRepository.InsertManyAsync(paperRepos);
        }

        [Authorize(ExamPermissions.Papers.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await paperRepoRepository.DeleteByPaperIdAsync(id);
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