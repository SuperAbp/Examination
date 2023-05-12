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
    /// <summary>
    /// 试卷题库管理
    /// </summary>
    [Authorize(ExamPermissions.PaperRepos.Default)]
    public class PaperRepoAppService : ExamAppService, IPaperRepoAppService
    {
        private readonly IPaperRepoRepository _examRepoRepository;
        private readonly IQuestionRepoRepository _questionRepoRepository;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="examRepoRepository"></param>
        public PaperRepoAppService(
            IPaperRepoRepository examRepoRepository, IQuestionRepoRepository questionRepoRepository)
        {
            _examRepoRepository = examRepoRepository;
            _questionRepoRepository = questionRepoRepository;
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns>结果</returns>
        public virtual async Task<PagedResultDto<PaperRepoListDto>> GetListAsync(GetPaperReposInput input)
        {
            await NormalizeMaxResultCountAsync(input);

            var examRepoQueryable = await _examRepoRepository.GetQueryableAsync();

            examRepoQueryable = examRepoQueryable.Where(e => e.PaperId == input.PaperId);

            var queryable =
                from er in examRepoQueryable
                join qr in (await _questionRepoRepository.GetQueryableAsync()) on er.QuestionRepositoryId equals qr.Id
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
                    CreationTime = er.CreationTime
                };

            long totalCount = await AsyncExecuter.CountAsync(queryable);

            var entities = await AsyncExecuter.ToListAsync(queryable
                .OrderBy(input.Sorting ?? PaperRepoConsts.DefaultSorting)
                .PageBy(input));

            var dtos = ObjectMapper.Map<List<PaperRepositoryDetail>, List<PaperRepoListDto>>(entities);

            return new PagedResultDto<PaperRepoListDto>(totalCount, dtos);
        }

        /// <summary>
        /// 获取修改
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public virtual async Task<GetPaperRepoForEditorOutput> GetEditorAsync(Guid id)
        {
            PaperRepo entity = await _examRepoRepository.GetAsync(id);

            return ObjectMapper.Map<PaperRepo, GetPaperRepoForEditorOutput>(entity);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(ExamPermissions.PaperRepos.Create)]
        public virtual async Task<PaperRepoListDto> CreateAsync(PaperRepoCreateDto input)
        {
            PaperRepo repository = ObjectMapper.Map<PaperRepoCreateDto, PaperRepo>(input); ;
            repository = await _examRepoRepository.InsertAsync(repository, true);
            return ObjectMapper.Map<PaperRepo, PaperRepoListDto>(repository);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(ExamPermissions.PaperRepos.Update)]
        public virtual async Task<PaperRepoListDto> UpdateAsync(Guid id, PaperRepoUpdateDto input)
        {
            var entity = await _examRepoRepository.GetAsync(id);
            entity = ObjectMapper.Map(input, entity);
            entity = await _examRepoRepository.UpdateAsync(entity);
            return ObjectMapper.Map<PaperRepo, PaperRepoListDto>(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [Authorize(ExamPermissions.PaperRepos.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _examRepoRepository.DeleteAsync(id);
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