using Microsoft.AspNetCore.Authorization;
using SuperAbp.Exam.QuestionManagement.Questions;
using SuperAbp.Exam.PaperManagement.Papers;
using SuperAbp.Exam.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using static SuperAbp.Exam.Admin.PaperManagement.Papers.PaperCreateOrUpdateDtoBase;

namespace SuperAbp.Exam.Admin.PaperManagement.Papers
{
    [Authorize(ExamPermissions.Papers.Default)]
    public class PaperAdminAppService(IPaperRepository paperRepository, PaperManager paperManager)
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
            Paper paper = await paperManager.CreateAsync(PaperType.FromValue(input.PaperType), input.Name,
                input.Sections.Sum(s => s.TotalScore), input.Sections.Sum(s => s.TotalCount));
            paper.Description = input.Description;

            // 创建Sections并保存聚合根
            await CreateOrUpdatePaperQuestionAsync(paper, input.Sections);

            return ObjectMapper.Map<Paper, PaperListDto>(paper);
        }

        [Authorize(ExamPermissions.Papers.Update)]
        public virtual async Task<PaperListDto> UpdateAsync(Guid id, PaperUpdateDto input)
        {
            Paper paper = await paperRepository.GetAsync(id);
            await paperManager.SetNameAsync(paper, input.Name);
            paper.Description = input.Description;

            // 更新Sections会自动重新计算Score和TotalQuestionCount
            await CreateOrUpdatePaperQuestionAsync(paper, input.Sections);

            return ObjectMapper.Map<Paper, PaperListDto>(paper);
        }

        protected virtual async Task CreateOrUpdatePaperQuestionAsync(Paper paper, PaperSectionDto[] dtos)
        {
            // 转换DTO为业务对象
            var sectionInfos = dtos.Select(dto => new PaperSectionCreateInfo
            {
                Id = dto.Id,
                Title = dto.Title,
                ScoreEach = dto.ScoreEach,
                TotalScore = dto.TotalScore,
                Order = dto.Order,
                TotalCount = dto.TotalCount,
                Remark = dto.Remark,
                QuestionInfos = paper.PaperType == PaperType.Fixed
                    ? dto.PaperQuestions.Select(q => new QuestionInfo
                    {
                        QuestionId = q.QuestionId,
                        Score = q.Score,
                        Order = q.Order
                    }).ToList()
                    : null,
                RuleInfos = paper.PaperType == PaperType.Random
                    ? dto.PaperQuestionRules.Select(r => new QuestionRuleInfo
                    {
                        Id = r.Id,
                        QuestionBankId = r.QuestionBankId,
                        QuestionType = (QuestionType)r.QuestionType,
                        Count = r.Count,
                        Score = r.Score,
                        Proportion = null
                    }).ToList()
                    : null
            }).ToList();

            // 通过聚合根更新Sections（使用流式API）
            await Task.FromResult(paper.UpdateSections(sectionInfos));

            // 保存聚合根（会级联保存所有内部实体）
            if (paper.Id == default)
            {
                await paperRepository.InsertAsync(paper);
            }
            else
            {
                await paperRepository.UpdateAsync(paper);
            }
        }

        [Authorize(ExamPermissions.Papers.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
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