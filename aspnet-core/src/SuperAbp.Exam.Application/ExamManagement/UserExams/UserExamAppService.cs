using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SuperAbp.Exam.ExamManagement.Exams;
using SuperAbp.Exam.PaperManagement.PaperRepos;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace SuperAbp.Exam.ExamManagement.UserExams
{
    [Authorize]
    public class UserExamAppService(
        IUserExamRepository userExamRepository,
        UserExamManager userExamManager,
        IExamRepository examRepository)
        : ExamAppService, IUserExamAppService
    {
        public async Task<Guid?> GetUnfinishedAsync()
        {
            var userExam = await userExamRepository.FindAsync(u => u.UserId == CurrentUser.GetId() && !u.Finished);
            return userExam?.Id;
        }

        public virtual async Task<UserExamDetailDto> GetAsync(Guid id)
        {
            UserExam entity = await userExamRepository.GetAsync(id);

            return ObjectMapper.Map<UserExam, UserExamDetailDto>(entity);
        }

        public virtual async Task<PagedResultDto<UserExamListDto>> GetListAsync(GetUserExamsInput input)
        {
            await NormalizeMaxResultCountAsync(input);

            IQueryable<UserExam> queryable = await userExamRepository.GetQueryableAsync();
            IQueryable<Examination> examQueryable = await examRepository.GetQueryableAsync();
            // TODO:性能较低，需要优化
            IQueryable<UserExamWithExam> result = from ue in queryable
                                                  join e in examQueryable on ue.ExamId equals e.Id
                                                  group new { ue, e } by ue.ExamId into g
                                                  select new UserExamWithExam
                                                  {
                                                      ExamId = g.Key,
                                                      ExamName = g.Max(m => m.e.Name),
                                                      Count = g.Count(),
                                                      LastTime = g.Max(m => m.ue.CreationTime),
                                                      MaxScore = g.Max(m => m.ue.TotalScore)
                                                  };
            int totalCount = await AsyncExecuter.CountAsync(result);
            List<UserExamWithExam> entities = await AsyncExecuter.ToListAsync(result
                .OrderBy(input.Sorting ?? UserExamConsts.DefaultSorting)
                .PageBy(input));

            List<UserExamListDto> dtos = ObjectMapper.Map<List<UserExamWithExam>, List<UserExamListDto>>(entities);
            return new PagedResultDto<UserExamListDto>(totalCount, dtos);
        }

        public virtual async Task<UserExamListDto> CreateAsync(UserExamCreateDto input)
        {
            if (await userExamRepository.AnyAsync(ue => ue.UserId == CurrentUser.GetId() && !ue.Finished))
            {
                throw new BusinessException(ExamDomainErrorCodes.ExistsUnfinishedExams);
            }
            Examination exam = await examRepository.GetAsync(input.ExamId);
            if (exam.StartTime > DateTime.Now || exam.EndTime < DateTime.Now)
            {
                throw new BusinessException(ExamDomainErrorCodes.OutOfExamTime);
            }
            UserExam userExam = new(GuidGenerator.Create(), input.ExamId, CurrentUser.GetId());
            await userExamManager.CreateQuestionsAsync(userExam.Id, input.ExamId);
            await userExamRepository.InsertAsync(userExam);
            return ObjectMapper.Map<UserExam, UserExamListDto>(userExam);
        }

        private async Task NormalizeMaxResultCountAsync(PagedAndSortedResultRequestDto input)
        {
            int? maxPageSize = (await SettingProvider.GetOrNullAsync(UserExamSettings.MaxPageSize))?.To<int>();
            if (maxPageSize.HasValue && input.MaxResultCount > maxPageSize.Value)
            {
                input.MaxResultCount = maxPageSize.Value;
            }
        }
    }
}