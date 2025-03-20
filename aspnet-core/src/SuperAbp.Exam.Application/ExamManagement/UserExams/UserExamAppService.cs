using Microsoft.AspNetCore.Authorization;
using SuperAbp.Exam.ExamManagement.Exams;
using SuperAbp.Exam.QuestionManagement.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using SuperAbp.Exam.ExamManagement.UserExamQuestions;
using SuperAbp.Exam.PaperManagement.Papers;
using SuperAbp.Exam.QuestionManagement.QuestionAnswers;
using SuperAbp.Exam.QuestionManagement.QuestionRepos;
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
        IExamRepository examRepository,
        IQuestionRepository questionRepository,
        IQuestionAnswerRepository questionAnswerRepository,
        IUserExamQuestionRepository userExamQuestionRepository)
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
            UserExam userExam = await userExamManager.CreateAsync(input.ExamId, CurrentUser.GetId());
            await userExamManager.CreateQuestionsAsync(userExam.Id, input.ExamId);
            await userExamRepository.InsertAsync(userExam);
            return ObjectMapper.Map<UserExam, UserExamListDto>(userExam);
        }

        public virtual async Task FinishedAsync(Guid id)
        {
            UserExam userExam = await userExamRepository.GetAsync(id);
            userExam.Finished = true;
            await userExamRepository.UpdateAsync(userExam);

            List<UserExamQuestionWithDetails> userExamQuestions = await userExamQuestionRepository.GetListAsync(userExamId: id);
            decimal totalScore = 0;
            foreach (UserExamQuestionWithDetails item in userExamQuestions)
            {
                Question question = await questionRepository.GetAsync(item.QuestionId);
                List<QuestionAnswer> questionAnswers = await questionAnswerRepository.GetListAsync(item.QuestionId);
                if ((question.QuestionType == QuestionType.SingleSelect || question.QuestionType == QuestionType.Judge)
                    && item.Answers == (questionAnswers.SingleOrDefault(a => a.Right)?.Id.ToString() ?? ""))
                {
                    totalScore += item.QuestionScore;
                }
                else if (question.QuestionType == QuestionType.MultiSelect
                        && (new HashSet<string>(item.Answers.Split(ExamConsts.Splitter)).SetEquals(questionAnswers.Where(a => a.Right).Select(a => a.Id.ToString()))))
                {
                    totalScore += item.QuestionScore;
                }
                else if (question.QuestionType == QuestionType.FillInTheBlanks)
                {
                    // TODO:一空多项，多空多项，无序
                }
            }

            userExam.TotalScore = totalScore;
            await userExamRepository.UpdateAsync(userExam);
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