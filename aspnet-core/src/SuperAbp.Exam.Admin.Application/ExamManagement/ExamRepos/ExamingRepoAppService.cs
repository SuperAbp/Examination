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
using SuperAbp.Exam.Admin.ExamManagement.Exams;
using SuperAbp.Exam.ExamManagement.Exams;
using SuperAbp.Exam.QuestionManagement.QuestionRepos;
using SuperAbp.Exam.Admin.QuestionManagement.QuestionRepos;
using SuperAbp.Exam.QuestionManagement.Questions;
using Volo.Abp.ObjectMapping;

namespace SuperAbp.Exam.Admin.ExamManagement.ExamRepos
{
    /// <summary>
    /// 考试题库管理
    /// </summary>
    [Authorize(ExamPermissions.ExamRepos.Default)]
    public class ExamingRepoAppService : ExamAppService, IExamingRepoAppService
    {
        private readonly IExamingRepoRepository _examRepoRepository;
        private  readonly  IQuestionRepoRepository _questionRepoRepository;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="examRepoRepository"></param>
        public ExamingRepoAppService(
            IExamingRepoRepository examRepoRepository, IQuestionRepoRepository questionRepoRepository)
        {
            _examRepoRepository = examRepoRepository;
            _questionRepoRepository = questionRepoRepository;
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns>结果</returns>
        public virtual async Task<PagedResultDto<ExamingRepoListDto>> GetListAsync(GetExamingReposInput input)
        {
            await NormalizeMaxResultCountAsync(input);

            var examRepoQueryable = await _examRepoRepository.GetQueryableAsync();

            examRepoQueryable = examRepoQueryable.Where(e => e.ExamingId == input.ExamingId);

            var queryable = 
                from er in examRepoQueryable
                join qr in (await _questionRepoRepository.GetQueryableAsync()) on er.QuestionRepositoryId equals qr.Id
                select new ExamingRepositoryDetail
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
                .OrderBy(input.Sorting ?? ExamingRepoConsts.DefaultSorting)
                .PageBy(input));
            
            var dtos = ObjectMapper.Map<List<ExamingRepositoryDetail>, List<ExamingRepoListDto>>(entities);

            return new PagedResultDto<ExamingRepoListDto>(totalCount, dtos);
        }

        /// <summary>
        /// 获取修改
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public virtual async Task<GetExamingRepoForEditorOutput> GetEditorAsync(Guid id)
        {
            ExamingRepo entity = await _examRepoRepository.GetAsync(id);

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
            ExamingRepo repository = ObjectMapper.Map<ExamingRepoCreateDto, ExamingRepo>(input); ;
            repository = await _examRepoRepository.InsertAsync(repository, true);
            return ObjectMapper.Map<ExamingRepo, ExamingRepoListDto>(repository);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(ExamPermissions.ExamRepos.Update)]
        public virtual async Task<ExamingRepoListDto> UpdateAsync(Guid id, ExamingRepoUpdateDto input)
        {
            var entity = await _examRepoRepository.GetAsync(id);
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
            var maxPageSize = (await SettingProvider.GetOrNullAsync(ExamingRepoSettings.MaxPageSize))?.To<int>();
            if (maxPageSize.HasValue && input.MaxResultCount > maxPageSize.Value)
            {
                input.MaxResultCount = maxPageSize.Value;
            }
        }
    }
}