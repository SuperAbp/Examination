using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using SuperAbp.Exam.ExamManagement.UserExams;
using Volo.Abp.Users;

namespace SuperAbp.Exam.ExamManagement.Exams
{
    [Authorize]
    public class ExaminationAppService(IExamRepository examRepository, IUserExamRepository userExamRepository) : ExamAppService, IExaminationAppService
    {
        public virtual async Task<ExamDetailDto> GetAsync(Guid id)
        {
            Examination examination = await examRepository.GetAsync(id);

            var dto = ObjectMapper.Map<Examination, ExamDetailDto>(examination);
            if (examination.MaxNumberOfTimes > 0)
            {
                int takenTimes = await userExamRepository.GetCountAsync(CurrentUser.GetId(), id);
                if (takenTimes >= examination.MaxNumberOfTimes)
                {
                    dto.MaxNumberOfTimesExceeded = true;
                }
            }
            return dto;
        }

        public virtual async Task<PagedResultDto<ExamListDto>> GetListAsync(GetExamsInput input)
        {
            await NormalizeMaxResultCountAsync(input);

            IQueryable<Examination> queryable = await examRepository.GetQueryableAsync();

            queryable = queryable
                .WhereIf(input.Status.HasValue, e => e.Status == ExaminationStatus.FromValue(input.Status.Value))
                .WhereIf(!input.Name.IsNullOrWhiteSpace(), e => e.Name.Contains(input.Name));

            long totalCount = await AsyncExecuter.CountAsync(queryable);

            List<Examination> entities = await AsyncExecuter.ToListAsync(queryable
                .OrderBy(input.Sorting ?? ExaminationConsts.DefaultSorting)
                .PageBy(input));

            List<ExamListDto> dtos = ObjectMapper.Map<List<Examination>, List<ExamListDto>>(entities);

            return new PagedResultDto<ExamListDto>(totalCount, dtos);
        }

        public virtual async Task<ListResultDto<ExamRankingDto>> GetRankingListAsync(Guid examId)
        {
            Examination exam = await examRepository.GetAsync(examId);
            if (exam.Status != ExaminationStatus.Completed)
            {
                throw new InvalidExamStatusException(exam.Status);
            }

            List<UserExamWithRanking> userExams = await userExamRepository.GetRankingListAsync(examId);
            List<ExamRankingDto> dtos = ObjectMapper.Map<List<UserExamWithRanking>, List<ExamRankingDto>>(userExams);
            RankingHelper.AssignRank(dtos, dto => dto.TotalScore, (dto, rank) => dto.Rank = rank);
            return new ListResultDto<ExamRankingDto>(dtos);
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