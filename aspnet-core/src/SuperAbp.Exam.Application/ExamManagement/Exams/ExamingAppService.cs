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
using SuperAbp.Exam.Admin.ExamManagement.Exams;

namespace SuperAbp.Exam.ExamManagement.Exams
{
    /// <summary>
    /// 考试管理
    /// </summary>
    [Authorize(ExamPermissions.Examings.Default)]
    public class ExamingAppService : ExamAppService, IExamingAppService
    {
        private readonly IExamingRepository _examingRepository;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="examingRepository"></param>
        public ExamingAppService(
            IExamingRepository examingRepository)
        {
            _examingRepository = examingRepository;
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

            queryable = queryable.WhereIf(!input.Name.IsNullOrWhiteSpace(), e => e.Name.Contains(input.Name));

            long totalCount = await AsyncExecuter.CountAsync(queryable);

            var entities = await AsyncExecuter.ToListAsync(queryable
                .OrderBy(input.Sorting ?? ExamingConsts.DefaultSorting)
                .PageBy(input));

            var dtos = ObjectMapper.Map<List<Examing>, List<ExamingListDto>>(entities);

            return new PagedResultDto<ExamingListDto>(totalCount, dtos);
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