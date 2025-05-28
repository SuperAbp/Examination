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
        protected IExamRepository ExamRepository { get; } = examRepository;

        public virtual async Task<PagedResultDto<ExamListDto>> GetListAsync(GetExamsInput input)
        {
            await NormalizeMaxResultCountAsync(input);

            IQueryable<Examination> queryable = await ExamRepository.GetQueryableAsync();

            queryable = queryable.WhereIf(!input.Name.IsNullOrWhiteSpace(), e => e.Name.Contains(input.Name));

            long totalCount = await AsyncExecuter.CountAsync(queryable);

            List<Examination> entities = await AsyncExecuter.ToListAsync(queryable
                .OrderBy(input.Sorting ?? ExaminationConsts.DefaultSorting)
                .PageBy(input));

            List<ExamListDto> dtos = ObjectMapper.Map<List<Examination>, List<ExamListDto>>(entities);

            return new PagedResultDto<ExamListDto>(totalCount, dtos);
        }

        public virtual async Task<ExamDetailDto> GetAsync(Guid id)
        {
            Examination entity = await ExamRepository.GetAsync(id);

            return ObjectMapper.Map<Examination, ExamDetailDto>(entity);
        }

        public virtual async Task<GetExamForEditorOutput> GetEditorAsync(Guid id)
        {
            Examination entity = await ExamRepository.GetAsync(id);

            return ObjectMapper.Map<Examination, GetExamForEditorOutput>(entity);
        }

        [Authorize(ExamPermissions.Exams.Create)]
        public virtual async Task<ExamListDto> CreateAsync(ExamCreateDto input)
        {
            Examination examination = new(GuidGenerator.Create(), input.PaperId, input.Name, input.Score, input.PassingScore, input.TotalTime)
            {
                Description = input.Description
            };
            examination.SetTime(input.StartTime, input.EndTime);
            examination = await ExamRepository.InsertAsync(examination);
            return ObjectMapper.Map<Examination, ExamListDto>(examination);
        }

        [Authorize(ExamPermissions.Exams.Update)]
        public virtual async Task<ExamListDto> UpdateAsync(Guid id, ExamUpdateDto input)
        {
            Examination examination = await ExamRepository.GetAsync(id);
            if (examination.Status != ExaminationStatus.Draft)
            {
                throw new InvalidExamStatusException(examination.Status);
            }
            examination.PaperId = input.PaperId;
            examination.Name = input.Name;
            examination.Score = input.Score;
            examination.PassingScore = input.PassingScore;
            examination.TotalTime = input.TotalTime;
            examination.Description = input.Description;
            examination.SetTime(input.StartTime, input.EndTime);
            examination = await ExamRepository.UpdateAsync(examination);
            return ObjectMapper.Map<Examination, ExamListDto>(examination);
        }

        [Authorize(ExamPermissions.Exams.Cancel)]
        public virtual async Task CancelAsync(Guid id)
        {
            Examination exam = await ExamRepository.GetAsync(id);
            exam.Status = ExaminationStatus.Cancelled;
            await ExamRepository.UpdateAsync(exam);
        }

        [Authorize(ExamPermissions.Exams.Publish)]
        public virtual async Task PublishAsync(Guid id)
        {
            Examination exam = await ExamRepository.GetAsync(id);
            if (exam.Status != ExaminationStatus.Draft)
            {
                throw new InvalidExamStatusException(exam.Status);
            }
            exam.Status = ExaminationStatus.Ongoing;
            await ExamRepository.UpdateAsync(exam);
        }

        [Authorize(ExamPermissions.Exams.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await ExamRepository.DeleteAsync(id);
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