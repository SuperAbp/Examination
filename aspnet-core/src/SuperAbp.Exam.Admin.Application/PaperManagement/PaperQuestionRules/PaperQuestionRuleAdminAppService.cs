using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SuperAbp.Exam.PaperManagement.PaperQuestionRules;
using SuperAbp.Exam.PaperManagement.Papers;
using SuperAbp.Exam.Permissions;
using SuperAbp.Exam.QuestionManagement.QuestionBanks;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.PaperManagement.PaperQuestionRules
{
    [Authorize(ExamPermissions.PaperQuestionRules.Default)]
    public class PaperQuestionRuleAdminAppService(
        IPaperRepository paperRepository,
        IPaperQuestionRuleRepository paperQuestionRuleRepository,
        IQuestionBankRepository questionBankRepository)
        : ExamAppService, IPaperQuestionRuleAdminAppService
    {
        public virtual async Task<PagedResultDto<PaperQuestionRuleListDto>> GetListAsync(GetPaperQuestionRulesInput input)
        {
            await NormalizeMaxResultCountAsync(input);

            var examRepoQueryable = await paperQuestionRuleRepository.GetQueryableAsync();

            var queryable =
                from er in examRepoQueryable
                join qr in (await questionBankRepository.GetQueryableAsync()) on er.QuestionBankId equals qr.Id
                select new PaperQuestionRuleWithDetails
                {
                    Id = er.Id,
                    QuestionBank = qr.Title,
                    QuestionBankId = er.QuestionBankId,
                    CreationTime = er.CreationTime
                };

            long totalCount = await AsyncExecuter.CountAsync(queryable);

            var entities = await AsyncExecuter.ToListAsync(queryable
                .OrderBy(input.Sorting ?? PaperQuestionRuleConsts.DefaultSorting)
                .PageBy(input));

            var dtos = ObjectMapper.Map<List<PaperQuestionRuleWithDetails>, List<PaperQuestionRuleListDto>>(entities);

            return new PagedResultDto<PaperQuestionRuleListDto>(totalCount, dtos);
        }

        public virtual async Task<GetPaperQuestionRuleForEditorOutput> GetEditorAsync(Guid id)
        {
            PaperQuestionRule entity = await paperQuestionRuleRepository.GetAsync(id);

            return ObjectMapper.Map<PaperQuestionRule, GetPaperQuestionRuleForEditorOutput>(entity);
        }

        [Authorize(ExamPermissions.PaperQuestionRules.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            // TODO: 删除试卷规则时，需同步更新试卷的总分和题目总数，暂时先注释掉删除功能
            //PaperQuestionRule paperRepo = await paperQuestionRuleRepository.GetAsync(id);
            //await paperQuestionRuleRepository.DeleteAsync(paperRepo);

            //Paper paper = await paperRepository.GetAsync(paperRepo.PaperId);
            //paper.Score = paper.Score - (paperRepo.SingleScore ?? 0) * (paperRepo.SingleCount ?? 0)
            //              + (paperRepo.MultiScore ?? 0) * (paperRepo.MultiCount ?? 0)
            //              + (paperRepo.JudgeScore ?? 0) * (paperRepo.JudgeCount ?? 0)
            //              + (paperRepo.BlankScore ?? 0) * (paperRepo.BlankCount ?? 0);
            //paper.TotalQuestionCount = paper.TotalQuestionCount - (paperRepo.SingleCount ?? 0) + (paperRepo.MultiCount ?? 0) + (paperRepo.JudgeCount ?? 0) + (paperRepo.BlankCount ?? 0);
            //await paperRepository.UpdateAsync(paper);
        }

        /// <summary>
        /// 规范最大记录数
        /// </summary>
        /// <param name="input">参数</param>
        /// <returns></returns>
        private async Task NormalizeMaxResultCountAsync(PagedAndSortedResultRequestDto input)
        {
            var maxPageSize = (await SettingProvider.GetOrNullAsync(PaperQuestionRuleSettings.MaxPageSize))?.To<int>();
            if (maxPageSize.HasValue && input.MaxResultCount > maxPageSize.Value)
            {
                input.MaxResultCount = maxPageSize.Value;
            }
        }
    }
}