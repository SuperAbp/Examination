using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SuperAbp.Exam.ExamManagement.Exams;
using Volo.Abp;
using Volo.Abp.Linq;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using SuperAbp.Exam.Permissions;

namespace SuperAbp.Exam.Admin.ExamManagement.Exams
{
    /// <summary>
    /// 考试管理
    /// </summary>
    [Authorize(ExamPermissions.Exams.Default)]
    public class ExaminationAppService : ExamAppService, IExaminationAppService
    {
        private readonly IExamRepository _examRepository;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="examRepository"></param>
        public ExaminationAppService(
            IExamRepository examRepository)
        {
            _examRepository = examRepository;
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public virtual async Task<ExamDetailDto> GetAsync(Guid id)
        {
            var entity = await _examRepository.GetAsync(id);

            return ObjectMapper.Map<Examination, ExamDetailDto>(entity);
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns>结果</returns>
        public virtual async Task<PagedResultDto<ExamListDto>> GetListAsync(GetExamsInput input)
        {
            await NormalizeMaxResultCountAsync(input);

            var queryable = await _examRepository.GetQueryableAsync();

            queryable = queryable.WhereIf(!input.Name.IsNullOrWhiteSpace(), e => e.Name.Contains(input.Name));

            long totalCount = await AsyncExecuter.CountAsync(queryable);

            var entities = await AsyncExecuter.ToListAsync(queryable
                .OrderBy(input.Sorting ?? ExaminationConsts.DefaultSorting)
                .PageBy(input));

            var dtos = ObjectMapper.Map<List<Examination>, List<ExamListDto>>(entities);

            return new PagedResultDto<ExamListDto>(totalCount, dtos);
        }

        /// <summary>
        /// 获取修改
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public virtual async Task<GetExamForEditorOutput> GetEditorAsync(Guid id)
        {
            var entity = await _examRepository.GetAsync(id);

            return ObjectMapper.Map<Examination, GetExamForEditorOutput>(entity);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(ExamPermissions.Exams.Create)]
        public virtual async Task<ExamListDto> CreateAsync(ExamCreateDto input)
        {
            var entity = ObjectMapper.Map<ExamCreateDto, Examination>(input);
            entity = await _examRepository.InsertAsync(entity, true);
            return ObjectMapper.Map<Examination, ExamListDto>(entity);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(ExamPermissions.Exams.Update)]
        public virtual async Task<ExamListDto> UpdateAsync(Guid id, ExamUpdateDto input)
        {
            var entity = await _examRepository.GetAsync(id);
            entity = ObjectMapper.Map(input, entity);
            entity = await _examRepository.UpdateAsync(entity);
            return ObjectMapper.Map<Examination, ExamListDto>(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        [Authorize(ExamPermissions.Exams.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _examRepository.DeleteAsync(s => s.Id == id);
        }

        /// <summary>
        /// 规范最大记录数
        /// </summary>
        /// <param name="input">参数</param>
        /// <returns></returns>
        private async Task NormalizeMaxResultCountAsync(PagedAndSortedResultRequestDto input)
        {
            var maxPageSize = (await SettingProvider.GetOrNullAsync(ExamSettings.MaxPageSize))?.To<int>();
            if (maxPageSize.HasValue && input.MaxResultCount > maxPageSize.Value)
            {
                input.MaxResultCount = maxPageSize.Value;
            }
        }
    }
}