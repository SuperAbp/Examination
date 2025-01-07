using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SuperAbp.Exam.PaperManagement.PaperRepos;
using SuperAbp.Exam.Permissions;
using SuperAbp.Exam.QuestionManagement.QuestionRepos;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.PaperManagement.PaperRepos
{
    [Authorize(ExamPermissions.PaperRepos.Default)]
    public class PaperRepoAdminAppService(
        IPaperRepoRepository examRepoRepository,
        IQuestionRepoRepository questionRepoRepository)
        : ExamAppService, IPaperRepoAdminAppService
    {
        public virtual async Task<PagedResultDto<PaperRepoListDto>> GetListAsync(GetPaperReposInput input)
        {
            await NormalizeMaxResultCountAsync(input);

            var examRepoQueryable = await examRepoRepository.GetQueryableAsync();

            examRepoQueryable = examRepoQueryable.Where(e => e.PaperId == input.PaperId);

            var queryable =
                from er in examRepoQueryable
                join qr in (await questionRepoRepository.GetQueryableAsync()) on er.QuestionRepositoryId equals qr.Id
                select new PaperRepositoryDetail
                {
                    Id = er.Id,
                    QuestionRepository = qr.Title,
                    QuestionRepositoryId = er.QuestionRepositoryId,
                    SingleCount = er.SingleCount,
                    SingleScore = er.SingleScore,
                    MultiCount = er.MultiCount,
                    MultiScore = er.MultiScore,
                    JudgeCount = er.JudgeCount,
                    JudgeScore = er.JudgeScore,
                    BlankCount = er.BlankCount,
                    BlankScore = er.BlankScore,
                    CreationTime = er.CreationTime
                };

            long totalCount = await AsyncExecuter.CountAsync(queryable);

            var entities = await AsyncExecuter.ToListAsync(queryable
                .OrderBy(input.Sorting ?? PaperRepoConsts.DefaultSorting)
                .PageBy(input));

            var dtos = ObjectMapper.Map<List<PaperRepositoryDetail>, List<PaperRepoListDto>>(entities);

            return new PagedResultDto<PaperRepoListDto>(totalCount, dtos);
        }

        public virtual async Task<GetPaperRepoForEditorOutput> GetEditorAsync(Guid id)
        {
            PaperRepo entity = await examRepoRepository.GetAsync(id);

            return ObjectMapper.Map<PaperRepo, GetPaperRepoForEditorOutput>(entity);
        }

        [Authorize(ExamPermissions.PaperRepos.Create)]
        public virtual async Task<PaperRepoListDto> CreateAsync(PaperRepoCreateDto input)
        {
            PaperRepo repository = new PaperRepo(GuidGenerator.Create(), input.PaperId, input.QuestionRepositoryId)
            {
                BlankCount = input.BlankCount,
                BlankScore = input.BlankScore,
                SingleCount = input.SingleCount,
                SingleScore = input.SingleScore,
                MultiCount = input.MultiCount,
                MultiScore = input.MultiScore,
                JudgeCount = input.JudgeCount,
                JudgeScore = input.JudgeScore
            };
            repository = await examRepoRepository.InsertAsync(repository);
            return ObjectMapper.Map<PaperRepo, PaperRepoListDto>(repository);
        }

        [Authorize(ExamPermissions.PaperRepos.Update)]
        public virtual async Task<PaperRepoListDto> UpdateAsync(Guid id, PaperRepoUpdateDto input)
        {
            var repository = await examRepoRepository.GetAsync(id);
            repository.BlankCount = input.BlankCount;
            repository.BlankScore = input.BlankScore;
            repository.SingleCount = input.SingleCount;
            repository.SingleScore = input.SingleScore;
            repository.MultiCount = input.MultiCount;
            repository.MultiScore = input.MultiScore;
            repository.JudgeCount = input.JudgeCount;
            repository.JudgeScore = input.JudgeScore;
            repository = await examRepoRepository.UpdateAsync(repository);
            return ObjectMapper.Map<PaperRepo, PaperRepoListDto>(repository);
        }

        [Authorize(ExamPermissions.PaperRepos.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await examRepoRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 规范最大记录数
        /// </summary>
        /// <param name="input">参数</param>
        /// <returns></returns>
        private async Task NormalizeMaxResultCountAsync(PagedAndSortedResultRequestDto input)
        {
            var maxPageSize = (await SettingProvider.GetOrNullAsync(PaperRepoSettings.MaxPageSize))?.To<int>();
            if (maxPageSize.HasValue && input.MaxResultCount > maxPageSize.Value)
            {
                input.MaxResultCount = maxPageSize.Value;
            }
        }
    }
}