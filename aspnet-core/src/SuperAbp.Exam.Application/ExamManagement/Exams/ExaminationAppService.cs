using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using SuperAbp.Exam.ExamManagement.UserExams;

namespace SuperAbp.Exam.ExamManagement.Exams
{
    [Authorize]
    public class ExaminationAppService(IExamRepository examRepository, IUserExamRepository userExamRepository) : ExamAppService, IExaminationAppService
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

            queryable = queryable
                .Where(e => e.Status == ExaminationStatus.Published)
                .WhereIf(!input.Name.IsNullOrWhiteSpace(), e => e.Name.Contains(input.Name));

            long totalCount = await AsyncExecuter.CountAsync(queryable);

            List<Examination> entities = await AsyncExecuter.ToListAsync(queryable
                .OrderBy(input.Sorting ?? ExaminationConsts.DefaultSorting)
                .PageBy(input));

            List<ExamListDto> dtos = ObjectMapper.Map<List<Examination>, List<ExamListDto>>(entities);

            return new PagedResultDto<ExamListDto>(totalCount, dtos);
        }

        public virtual async Task<List<ExamRankingDto>> GetRankingListAsync(Guid examId)
        {
            await examRepository.GetAsync(examId);

            List<UserExamWithUser> userExams = await userExamRepository.GetRankingListAsync(examId);

            return ObjectMapper.Map<List<UserExamWithUser>, List<ExamRankingDto>>(userExams);
        }

        private async Task NormalizeMaxResultCountAsync(PagedAndSortedResultRequestDto input)
        {
            int? maxPageSize = (await SettingProvider.GetOrNullAsync(ExamSettings.MaxPageSize))?.To<int>();
            if (maxPageSize.HasValue && input.MaxResultCount > maxPageSize.Value)
            {
                input.MaxResultCount = maxPageSize.Value;
            }
        }
    }
}