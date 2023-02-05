using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SuperAbp.Exam.ExamManagement.ExamRepos;
using Volo.Abp;
using Volo.Abp.Linq;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using SuperAbp.Exam.Permissions;
using SuperAbp.Exam.ExamManagement.Exams;

namespace SuperAbp.Exam.Admin.ExamManagement.Exams
{
    /// <summary>
    /// 考试管理
    /// </summary>
    [Authorize(ExamPermissions.Examings.Default)]
    public class ExamingAppService : ExamAppService, IExamingAppService
    {
        private readonly IExamingRepository _examingRepository;
        private  readonly  IExamingRepoRepository _examingRepoRepository;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="examingRepository"></param>
        /// <param name="examingRepoRepository"></param>
        public ExamingAppService(
            IExamingRepository examingRepository, IExamingRepoRepository examingRepoRepository)
        {
            _examingRepository = examingRepository;
            _examingRepoRepository = examingRepoRepository;
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public virtual async Task<ExamingDetailDto> GetAsync(Guid id)
        {
            Examing entity = await _examingRepository.GetAsync(id);

            return ObjectMapper.Map<Examing, ExamingDetailDto>(entity);
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns>结果</returns>
        public virtual async Task<PagedResultDto<ExamingListDto>> GetListAsync(GetExamingsInput input)
        {
            await NormalizeMaxResultCountAsync(input);

            var queryable = await _examingRepository.GetQueryableAsync();

            long totalCount = await AsyncExecuter.CountAsync(queryable);

            var entities = await AsyncExecuter.ToListAsync(queryable
                .OrderBy(input.Sorting ?? "Id DESC")
                .PageBy(input));

            var dtos = ObjectMapper.Map<List<Examing>, List<ExamingListDto>>(entities);

            return new PagedResultDto<ExamingListDto>(totalCount, dtos);
        }

        /// <summary>
        /// 获取修改
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public virtual async Task<GetExamingForEditorOutput> GetEditorAsync(Guid id)
        {
            Examing entity = await _examingRepository.GetAsync(id);

            return ObjectMapper.Map<Examing, GetExamingForEditorOutput>(entity);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(ExamPermissions.Examings.Create)]
        public virtual async Task<ExamingListDto> CreateAsync(ExamingCreateDto input)
        {
            if (await _examingRepository.ExistsByNameAsync(input.Name.Trim()))
            {
                throw new UserFriendlyException(L["ExamingExists"]);
            }
            var entity = ObjectMapper.Map<ExamingCreateDto, Examing>(input);
            entity = await _examingRepository.InsertAsync(entity, true);
            return ObjectMapper.Map<Examing, ExamingListDto>(entity);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(ExamPermissions.Examings.Update)]
        public virtual async Task<ExamingListDto> UpdateAsync(Guid id, ExamingUpdateDto input)
        {
            var entity = await _examingRepository.GetAsync(id);
            entity = ObjectMapper.Map(input, entity);
            entity = await _examingRepository.UpdateAsync(entity);
            return ObjectMapper.Map<Examing, ExamingListDto>(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [Authorize(ExamPermissions.Examings.Delete)]
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
            var maxPageSize = (await SettingProvider.GetOrNullAsync(ExamingSettings.MaxPageSize))?.To<int>();
            if (maxPageSize.HasValue && input.MaxResultCount > maxPageSize.Value)
            {
                input.MaxResultCount = maxPageSize.Value;
            }
        }
    }
}