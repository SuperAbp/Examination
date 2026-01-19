using Microsoft.AspNetCore.Authorization;
using SuperAbp.Exam.PaperManagement.PaperQuestions;
using SuperAbp.Exam.PaperManagement.Papers;
using SuperAbp.Exam.PaperManagement.PaperSections;
using SuperAbp.Exam.Permissions;
using SuperAbp.Exam.QuestionManagement.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using static SuperAbp.Exam.Admin.PaperManagement.Papers.PaperCreateOrUpdateDtoBase;
using static SuperAbp.Exam.Admin.PaperManagement.Papers.PaperCreateOrUpdateDtoBase.PaperSectionDto;

namespace SuperAbp.Exam.Admin.PaperManagement.Papers
{
    [Authorize(ExamPermissions.Papers.Default)]
    public class PaperAdminAppService(IPaperRepository paperRepository, PaperManager paperManager, IQuestionRepository questionRepository)
        : ExamAppService, IPaperAdminAppService
    {
        public virtual async Task<PagedResultDto<PaperListDto>> GetListAsync(GetPapersInput input)
        {
            await NormalizeMaxResultCountAsync(input);

            var queryable = await paperRepository.GetQueryableAsync();

            queryable = queryable.WhereIf(!input.Name.IsNullOrWhiteSpace(), e => e.Name.Contains(input.Name));

            long totalCount = await AsyncExecuter.CountAsync(queryable);

            var entities = await AsyncExecuter.ToListAsync(queryable
                .OrderByDescending(p => p.Id)
                .PageBy(input));

            var dtos = ObjectMapper.Map<List<Paper>, List<PaperListDto>>(entities);

            return new PagedResultDto<PaperListDto>(totalCount, dtos);
        }

        public virtual async Task<GetPaperForEditorOutput> GetEditorAsync(Guid id)
        {
            Paper entity = await paperRepository.GetAsync(id);
            var dto = ObjectMapper.Map<Paper, GetPaperForEditorOutput>(entity);
            return ObjectMapper.Map<Paper, GetPaperForEditorOutput>(entity);
        }

        [Authorize(ExamPermissions.Papers.Create)]
        public virtual async Task<PaperListDto> CreateAsync(PaperCreateDto input)
        {
            bool manualReview = false;
            PaperType paperType = PaperType.FromValue(input.PaperType);
            //if (PaperType.Random == paperType)
            //{
            //    manualReview = input.Sections.Any(s => s.PaperQuestionRules.Any(r => r.QuestionType == QuestionType.FillInTheBlanks));
            //}
            //else
            //{
            //    List<Guid> questionIds = input.Sections.SelectMany(s => s.PaperQuestions).Select(q => q.QuestionId).Distinct().ToList();
            //    manualReview = await questionRepository.ExistsQuestionTypeAsync(QuestionType.FillInTheBlanks.Value, questionIds);
            //}
            Paper paper = await paperManager.CreateAsync(paperType, input.Name, manualReview);
            paper.Description = input.Description;

            CreateOrUpdatePaperQuestion(paper, input.Sections);

            await paperRepository.InsertAsync(paper);

            return ObjectMapper.Map<Paper, PaperListDto>(paper);
        }

        [Authorize(ExamPermissions.Papers.Update)]
        public virtual async Task<PaperListDto> UpdateAsync(Guid id, PaperUpdateDto input)
        {
            Paper paper = await paperRepository.GetAsync(id);
            await paperManager.SetNameAsync(paper, input.Name);
            paper.Description = input.Description;

            RemoveOldSections(paper, input.Sections);
            CreateOrUpdatePaperQuestion(paper, input.Sections);
            if (paper.PaperType == PaperType.Random)
            {
                paper.ManualReview = input.Sections.Any(s => s.PaperQuestionRules.Any(r => r.QuestionType == QuestionType.FillInTheBlanks));
            }
            else
            {
                List<Guid> questionIds = input.Sections.SelectMany(s => s.PaperQuestions).Select(q => q.QuestionId).Distinct().ToList();
                paper.ManualReview = await questionRepository.ExistsQuestionTypeAsync(QuestionType.FillInTheBlanks.Value, questionIds);
            }

            await paperRepository.UpdateAsync(paper);
            return ObjectMapper.Map<Paper, PaperListDto>(paper);
        }

        protected virtual void RemoveOldSections(Paper paper, PaperSectionDto[] sections)
        {
            Guid[] sectionIds = sections.Where(s => s.Id.HasValue).Select(s => s.Id!.Value).ToArray();
            List<Guid> sectionsToBeRemoved = paper.PaperSections.Where(s => !sectionIds.Any(i => i == s.Id)).Select(s => s.Id).ToList();
            paper.RemoveSections(sectionsToBeRemoved);
            List<PaperSection> existedSections = paper.PaperSections.Where(s => sectionIds.Any(i => i == s.Id)).ToList();
            foreach (PaperSection section in existedSections)
            {
                PaperSectionDto? sectionDto = sections.SingleOrDefault(s => s.Id == section.Id);
                if (sectionDto == null)
                {
                    continue;
                }

                if (PaperType.Fixed == paper.PaperType)
                {
                    Guid[] questionIds = sectionDto.PaperQuestions.Select(q => q.QuestionId).ToArray();
                    List<Guid> questionsToBeRemoved = section.PaperQuestions
                        .Where(q => !questionIds.Any(i => i == q.QuestionId))
                        .Select(q => q.Id)
                        .ToList();
                    section.RemoveQuestions(questionsToBeRemoved);
                }
                else
                {
                    Guid[] ruleIds = sectionDto.PaperQuestionRules.Where(r => r.Id.HasValue).Select(r => r.Id!.Value).ToArray();
                    List<Guid> rulesToBeRemoved = section.PaperQuestionRules
                        .Where(r => !ruleIds.Any(i => i == r.Id))
                        .Select(r => r.Id)
                        .ToList();
                    section.RemoveRules(rulesToBeRemoved);
                }
            }
        }

        protected virtual void CreateOrUpdatePaperQuestion(Paper paper, PaperSectionDto[] sections)
        {
            foreach (PaperSectionDto sectionDto in sections)
            {
                Guid sectionId = sectionDto.Id ?? GuidGenerator.Create();
                if (sectionDto.Id.HasValue)
                {
                    paper.UpdateSection(sectionId, sectionDto.Title,
                        sectionDto.ScoreEach, sectionDto.Order);
                }
                else
                {
                    paper.AddSection(sectionId, sectionDto.Title,
                        sectionDto.ScoreEach, sectionDto.Order);
                }

                if (PaperType.Fixed == paper.PaperType)
                {
                    foreach (PaperQuestionDto questionDto in sectionDto.PaperQuestions)
                    {
                        PaperSection section = paper.PaperSections.First(s => s.Id == sectionId);
                        PaperQuestion? existingQuestion = section.PaperQuestions.FirstOrDefault(q => q.QuestionId == questionDto.QuestionId);
                        if (existingQuestion != null)
                        {
                            paper.UpdateQuestion(sectionId, existingQuestion.Id, questionDto.Score, questionDto.Order);
                        }
                        else
                        {
                            paper.AddQuestion(sectionId, GuidGenerator.Create(), questionDto.QuestionId, questionDto.Score, questionDto.Order);
                        }
                    }
                }
                else
                {
                    foreach (PaperQuestionRuleDto ruleDto in sectionDto.PaperQuestionRules)
                    {
                        if (ruleDto.Id.HasValue)
                        {
                            paper.UpdateRule(sectionId, ruleDto.Id.Value, ruleDto.QuestionBankId,
                                QuestionType.FromValue(ruleDto.QuestionType), ruleDto.Count, ruleDto.Score);
                        }
                        else
                        {
                            paper.AddRule(sectionId, GuidGenerator.Create(), ruleDto.QuestionBankId,
                                QuestionType.FromValue(ruleDto.QuestionType), ruleDto.Count, ruleDto.Score);
                        }
                    }
                }
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