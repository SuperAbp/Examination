using Microsoft.AspNetCore.Authorization;
using SuperAbp.Exam.Admin.ExamManagement.UserExams;
using SuperAbp.Exam.ExamManagement.Exams;
using SuperAbp.Exam.ExamManagement.UserExams;
using SuperAbp.Exam.Jobs.SubmittedUserExam;
using SuperAbp.Exam.PaperManagement.Papers;
using SuperAbp.Exam.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.Admin.ExamManagement.Exams
{
    [Authorize(ExamPermissions.Exams.Default)]
    public class ExaminationAdminAppService(IPaperRepository paperRepository,
        IExamRepository examRepository,
        IUserExamRepository userExamRepository,
        IBackgroundJobManager backgroundJobManager)
        : ExamAppService, IExaminationAdminAppService
    {
        protected IExamRepository ExamRepository { get; } = examRepository;
        public IUserExamRepository UserExamRepository { get; } = userExamRepository;
        public IBackgroundJobManager BackgroundJobManager { get; } = backgroundJobManager;

        public virtual async Task<PagedResultDto<ExamListDto>> GetListAsync(GetExamsInput input)
        {
            await NormalizeMaxResultCountAsync(input);

            IQueryable<Examination> queryable = await ExamRepository.GetQueryableAsync();

            queryable = queryable
                .WhereIf(!input.Name.IsNullOrWhiteSpace(), e => e.Name.Contains(input.Name))
                .WhereIf(input.Status.HasValue, e => input.Status.Value == e.Status.Value);

            long totalCount = await AsyncExecuter.CountAsync(queryable);
            List<Examination> entities = await AsyncExecuter.ToListAsync(queryable
                .OrderBy(input.Sorting ?? ExaminationConsts.DefaultSorting)
                .PageBy(input));

            List<ExamListDto> dtos = ObjectMapper.Map<List<Examination>, List<ExamListDto>>(entities);

            return new PagedResultDto<ExamListDto>(totalCount, dtos);
        }

        public virtual async Task<ListResultDto<ExamUserExamDto>> GetExamUserExamsAsync(Guid examId)
        {
            List<UserExamWithUser> userExams = await UserExamRepository.GetListByExamIdAsync(examId);
            List<ExamUserExamDto> dtos = ObjectMapper.Map<List<UserExamWithUser>, List<ExamUserExamDto>>(userExams);
            RankingHelper.AssignRank(dtos, dto => dto.MaxScore, (dto, rank) => dto.Rank = rank);
            return new ListResultDto<ExamUserExamDto>(dtos);
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
            Paper paper = await paperRepository.GetAsync(input.PaperId);
            Examination examination = new(GuidGenerator.Create(), input.PaperId, input.Name, input.Score,
                input.PassingScore, input.TotalTime, AnswerMode.FromValue(input.AnswerMode),
                input.RandomOrderOfOption, paper.ManualReview, ReviewMode.FromValue(input.ReviewMode))
            {
                Description = input.Description,
                MaxNumberOfTimes = input.MaxNumberOfTimes
            };
            if (input.Published)
            {
                examination.Publish();
            }
            examination.SetTime(input.StartTime, input.EndTime);
            examination = await ExamRepository.InsertAsync(examination);
            return ObjectMapper.Map<Examination, ExamListDto>(examination);
        }

        [Authorize(ExamPermissions.Exams.Update)]
        public virtual async Task<ExamListDto> UpdateAsync(Guid id, ExamUpdateDto input)
        {
            Examination examination = await ExamRepository.GetAsync(id);
            if (!examination.CanUpdate())
            {
                throw new InvalidExamStatusException(examination.Status);
            }
            if (input.Published)
            {
                examination.Publish();
            }
            examination.MaxNumberOfTimes = input.MaxNumberOfTimes;
            examination.PaperId = input.PaperId;
            examination.Name = input.Name;
            examination.Score = input.Score;
            examination.PassingScore = input.PassingScore;
            examination.TotalTime = input.TotalTime;
            examination.Description = input.Description;
            examination.AnswerMode = AnswerMode.FromValue(input.AnswerMode);
            examination.RandomOrderOfOption = input.RandomOrderOfOption;
            examination.ReviewMode = ReviewMode.FromValue(input.ReviewMode);
            examination.SetTime(input.StartTime, input.EndTime);
            examination = await ExamRepository.UpdateAsync(examination);
            return ObjectMapper.Map<Examination, ExamListDto>(examination);
        }

        [Authorize(ExamPermissions.Exams.Cancel)]
        public virtual async Task CancelAsync(Guid id)
        {
            Examination exam = await ExamRepository.GetAsync(id);
            exam.Cancel();
            await ExamRepository.UpdateAsync(exam);
        }

        [Authorize(ExamPermissions.Exams.Terminate)]
        public virtual async Task TerminateAsync(Guid id)
        {
            Examination exam = await ExamRepository.GetAsync(id);
            exam.Terminate(Clock.Now);
            await ExamRepository.UpdateAsync(exam);
            await BackgroundJobManager.EnqueueAsync(new SubmitUserExamArgs()
            {
                ExamId = id,
                TenantId = CurrentTenant.Id
            });
        }

        [Authorize(ExamPermissions.Exams.Complete)]
        public async Task CompleteAsync(Guid id)
        {
            Examination exam = await ExamRepository.GetAsync(id);
            if (await UserExamRepository.AnyAsync(ue => ue.ExamId == id && new UserExamStatus[] { UserExamStatus.Waiting, UserExamStatus.InProgress, UserExamStatus.Submitted }.Contains(ue.Status)))
            {
                throw new UnfinishedGradingException();
            }
            exam.Complete();
            await ExamRepository.UpdateAsync(exam);
        }

        [Authorize(ExamPermissions.Exams.Publish)]
        public virtual async Task PublishAsync(Guid id)
        {
            Examination exam = await ExamRepository.GetAsync(id);
            exam.Publish();
            await ExamRepository.UpdateAsync(exam);
        }

        [Authorize(ExamPermissions.Exams.Invalidate)]
        public virtual async Task InvalidateAsync(Guid id)
        {
            Examination exam = await ExamRepository.GetAsync(id);
            exam.Invalidate();
            await ExamRepository.UpdateAsync(exam);

            List<UserExam> userExams = await UserExamRepository.GetListAsync(examId: id);
            foreach (UserExam userExam in userExams)
            {
                userExam.Invalidate();
            }
            if (userExams.Count > 0)
            {
                await UserExamRepository.UpdateManyAsync(userExams);
            }
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