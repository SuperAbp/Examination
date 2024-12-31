using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SuperAbp.Exam.ExamManagement.Exams;
using Volo.Abp.Application.Dtos;
using SuperAbp.Exam.Permissions;

namespace SuperAbp.Exam.Admin.ExamManagement.Exams
{
    [Authorize(ExamPermissions.Exams.Default)]
    public class ExaminationAdminAppService(IExamRepository examRepository) : ExamAppService, IExaminationAdminAppService
    {
        public virtual async Task<ExamDetailDto> GetAsync(Guid id)
        {
            Examination entity = await examRepository.GetAsync(id);

            return ObjectMapper.Map<Examination, ExamDetailDto>(entity);
        }

        public virtual async Task<PagedResultDto<ExamListDto>> GetListAsync(GetExamsInput input)
        {
            await NormalizeMaxResultCountAsync(input);

            IQueryable<Examination> queryable = await examRepository.GetQueryableAsync();

            queryable = queryable.WhereIf(!input.Name.IsNullOrWhiteSpace(), e => e.Name.Contains(input.Name));

            long totalCount = await AsyncExecuter.CountAsync(queryable);

            List<Examination> entities = await AsyncExecuter.ToListAsync(queryable
                .OrderBy(input.Sorting ?? ExaminationConsts.DefaultSorting)
                .PageBy(input));

            List<ExamListDto> dtos = ObjectMapper.Map<List<Examination>, List<ExamListDto>>(entities);

            return new PagedResultDto<ExamListDto>(totalCount, dtos);
        }

        public virtual async Task<GetExamForEditorOutput> GetEditorAsync(Guid id)
        {
            Examination entity = await examRepository.GetAsync(id);

            return ObjectMapper.Map<Examination, GetExamForEditorOutput>(entity);
        }

        [Authorize(ExamPermissions.Exams.Create)]
        public virtual async Task<ExamListDto> CreateAsync(ExamCreateDto input)
        {
            Examination examination = new Examination(GuidGenerator.Create(), input.PaperId, input.Name, input.Score, input.PassingScore, input.TotalTime)
            {
                Description = input.Description
            };
            examination.SetTime(input.StartTime, input.EndTime);
            examination = await examRepository.InsertAsync(examination, true);
            return ObjectMapper.Map<Examination, ExamListDto>(examination);
        }

        [Authorize(ExamPermissions.Exams.Update)]
        public virtual async Task<ExamListDto> UpdateAsync(Guid id, ExamUpdateDto input)
        {
            Examination examination = await examRepository.GetAsync(id);
            examination.PaperId = input.PaperId;
            examination.Name = input.Name;
            examination.Score = input.Score;
            examination.TotalTime = input.TotalTime;
            examination.Description = input.Description;
            examination.SetTime(input.StartTime, input.EndTime);
            examination = await examRepository.UpdateAsync(examination);
            return ObjectMapper.Map<Examination, ExamListDto>(examination);
        }

        [Authorize(ExamPermissions.Exams.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await examRepository.DeleteAsync(id);
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