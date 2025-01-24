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
            Paper paper = await paperManager.CreateAsync(input.Name, GetTotalScore(input.Repositories));
            paper.Description = input.Description;
            paper.TotalQuestionCount = input.Repositories.Sum(p => p.SingleCount + p.MultiCount + p.JudgeCount + p.BlankCount) ?? 0;
            paper = await paperRepository.InsertAsync(paper);
            await CreateOrUpdatePaperRepoAsync(paper.Id, input.Repositories);
            return ObjectMapper.Map<Paper, PaperListDto>(paper);
        }

        [Authorize(ExamPermissions.Papers.Update)]
        public virtual async Task<PaperListDto> UpdateAsync(Guid id, PaperUpdateDto input)
        {
            Paper paper = await paperRepository.GetAsync(id);
            await paperManager.SetNameAsync(paper, input.Name);
            paper.Score = GetTotalScore(input.Repositories);
            paper.Description = input.Description;
            paper = await paperRepository.UpdateAsync(paper);
            await CreateOrUpdatePaperRepoAsync(id, input.Repositories);
            return ObjectMapper.Map<Paper, PaperListDto>(paper);
        }

        protected virtual decimal GetTotalScore(PaperCreateOrUpdatePaperRepoDto[] dtos)
        {
            return dtos.Sum(r => (r.SingleScore ?? 0) * (r.SingleCount ?? 0)
                                 + (r.MultiScore ?? 0) * (r.MultiCount ?? 0)
                                 + (r.JudgeScore ?? 0) * (r.JudgeCount ?? 0)
                                 + (r.BlankScore ?? 0) * (r.BlankCount ?? 0));
        }

        protected virtual async Task CreateOrUpdatePaperRepoAsync(Guid paperId, PaperCreateOrUpdatePaperRepoDto[] dtos)
        {
            List<PaperRepo> paperRepos = await paperRepoRepository.GetListAsync(paperId: paperId);
            List<PaperRepo> newPaperRepos = [];
            List<PaperRepo> updatePaperRepos = [];
            foreach (PaperCreateOrUpdatePaperRepoDto dto in dtos)
            {
                if (dto.Id.HasValue)
                {
                    PaperRepo questionAnswer = paperRepos.Single(a => a.Id == dto.Id.Value);
                    questionAnswer.BlankCount = dto.BlankCount;
                    questionAnswer.BlankScore = dto.BlankScore;
                    questionAnswer.SingleCount = dto.SingleCount;
                    questionAnswer.SingleScore = dto.SingleScore;
                    questionAnswer.MultiCount = dto.MultiCount;
                    questionAnswer.MultiScore = dto.MultiScore;
                    questionAnswer.JudgeCount = dto.JudgeCount;
                    questionAnswer.JudgeScore = dto.JudgeScore;
                    updatePaperRepos.Add(questionAnswer);
                }
                else
                {
                    newPaperRepos.Add(new PaperRepo(GuidGenerator.Create(), paperId, dto.QuestionRepositoryId)
                    {
                        BlankCount = dto.BlankCount,
                        BlankScore = dto.BlankScore,
                        SingleCount = dto.SingleCount,
                        SingleScore = dto.SingleScore,
                        MultiCount = dto.MultiCount,
                        MultiScore = dto.MultiScore,
                        JudgeCount = dto.JudgeCount,
                        JudgeScore = dto.JudgeScore
                    });
                }
            }
            await paperRepoRepository.InsertManyAsync(newPaperRepos);
            await paperRepoRepository.UpdateManyAsync(updatePaperRepos);
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