using Microsoft.AspNetCore.Authorization;
using SuperAbp.Exam.ExamManagement.Exams;
using SuperAbp.Exam.QuestionManagement.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SuperAbp.Exam.ExamManagement.UserExamQuestions;
using SuperAbp.Exam.Jobs.UserExamCreateQuestion;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Timing;
using Volo.Abp.Users;
using static SuperAbp.Exam.ExamManagement.UserExams.UserExamDetailDto.QuestionDto;
using SuperAbp.Exam.KnowledgePoints;
using Volo.Abp;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.ObjectMapping;
using SuperAbp.Exam.QuestionManagement.Questions.QuestionAnswers;

namespace SuperAbp.Exam.ExamManagement.UserExams
{
    [Authorize]
    public class UserExamAppService(
        IClock clock,
        IUserExamRepository userExamRepository,
        IExamRepository examRepository,
        UserExamManager userExamManager,
        IQuestionRepository questionRepository,
        QuestionManager questionManager,
        IBackgroundJobManager backgroundJobManager,
        IQuestionAnswerRepository questionAnswerRepository)
        : ExamAppService, IUserExamAppService
    {
        protected IUserExamRepository UserExamRepository { get; } = userExamRepository;
        protected IExamRepository ExamRepository { get; } = examRepository;
        protected IBackgroundJobManager BackgroundJobManager { get; } = backgroundJobManager;

        public async Task<Guid?> GetUnfinishedAsync()
        {
            var userExam = await UserExamRepository.FindAsync(u => u.UserId == CurrentUser.GetId() && u.Status == UserExamStatus.InProgress);
            return userExam?.Id;
        }

        public virtual async Task<UserExamDetailDto> GetAsync(Guid id)
        {
            UserExam userExam = await UserExamRepository.GetAsync(id);
            List<Guid> questionIds = userExam.Questions.Select(q => q.QuestionId).ToList();
            List<Question> questions = await questionRepository.GetByIdsAsync(questionIds);
            UserExamDetailDto dto = ObjectMapper.Map<UserExam, UserExamDetailDto>(userExam);
            List<UserExamDetailDto.QuestionDto> questionDtos = [];
            foreach (Question question in questions)
            {
                var questionDto = ObjectMapper.Map<Question, UserExamDetailDto.QuestionDto>(question);
                UserExamQuestion userExamQuestion = userExam.Questions.Single(q => q.QuestionId == question.Id);
                questionDto.Right = userExamQuestion.Right;
                questionDto.Answers = userExamQuestion.Answers;
                questionDto.QuestionScore = userExamQuestion.QuestionScore;
                // TODO:batch query
                List<KnowledgePoint> knowledgePoints = await questionManager.GetKnowledgePointsAsync(question.Id);
                if (knowledgePoints.Count > 0)
                {
                    questionDto.KnowledgePoints = knowledgePoints.Select(kp => kp.Name).ToArray();
                }
                List<OptionDto> answerDtos = [];
                foreach (QuestionAnswer answer in question.Answers)
                {
                    OptionDto optionDto = new()
                    {
                        Id = answer.Id,
                        Content = answer.Content,
                    };
                    if (userExam.IsSubmitted())
                    {
                        optionDto.Right = answer.Right;
                    }
                    answerDtos.Add(optionDto);
                }
                questionDto.Options = answerDtos;
                questionDtos.Add(questionDto);
            }
            dto.Questions = questionDtos;
            return dto;
        }

        public virtual async Task<PagedResultDto<UserExamListDto>> GetListAsync(GetUserExamsInput input)
        {
            await NormalizeMaxResultCountAsync(input);

            int totalCount = await UserExamRepository.GetCountAsync(CurrentUser.GetId());
            List<UserExamWithDetails> entities = await UserExamRepository.GetListWithDetailAsync(
                input.Sorting ?? UserExamConsts.DefaultSorting, input.SkipCount, input.MaxResultCount,
                CurrentUser.GetId());

            List<UserExamListDto> dtos = ObjectMapper.Map<List<UserExamWithDetails>, List<UserExamListDto>>(entities);
            return new PagedResultDto<UserExamListDto>(totalCount, dtos);
        }

        public virtual async Task<UserExamListDto> CreateAsync(UserExamCreateDto input)
        {
            List<UserExam> userExams = await UserExamRepository.GetListAsync(userId: CurrentUser.GetId(), examId: input.ExamId);
            UserExam? inProgressUserExam = userExams.SingleOrDefault(u =>
                new[] { UserExamStatus.Waiting, UserExamStatus.InProgress }.Contains(u.Status));
            if (inProgressUserExam is not null)
            {
                return ObjectMapper.Map<UserExam, UserExamListDto>(inProgressUserExam);
            }
            UserExam userExam = await userExamManager.CreateAsync(input.ExamId, CurrentUser.GetId());
            await UserExamRepository.InsertAsync(userExam);
            await BackgroundJobManager.EnqueueAsync(new UserExamCreateQuestionArgs()
            {
                UserExamId = userExam.Id
            });
            return ObjectMapper.Map<UserExam, UserExamListDto>(userExam);
        }

        public virtual async Task StartAsync(Guid id)
        {
            UserExam userExam = await UserExamRepository.GetAsync(id);
            if (userExam.Status != UserExamStatus.Waiting)
            {
                throw new InvalidUserExamStatusException(userExam.Status);
            }
            userExam.Status = UserExamStatus.InProgress;
            await UserExamRepository.UpdateAsync(userExam);
        }

        public virtual async Task AnswerAsync(Guid id, UserExamAnswerDto input)
        {
            UserExam userExam = await UserExamRepository.GetAsync(id);
            Examination examination = await ExamRepository.GetAsync(userExam.ExamId);
            if (examination.Status != ExaminationStatus.Published)
            {
                throw new InvalidExamStatusException(examination.Status);
            }

            userExam.AnswerQuestion(input.QuestionId, input.Answers);
            await UserExamRepository.UpdateAsync(userExam);
        }

        public virtual async Task FinishedAsync(Guid id, List<UserExamAnswerDto> input)
        {
            UserExam userExam = await UserExamRepository.GetAsync(id);
            Examination examination = await ExamRepository.GetAsync(userExam.ExamId);
            if (examination.Status != ExaminationStatus.Published)
            {
                throw new InvalidExamStatusException(examination.Status);
            }
            if (userExam.Status != UserExamStatus.InProgress)
            {
                throw new InvalidUserExamStatusException(userExam.Status);
            }
            userExam.FinishedTime = clock.Now;
            // TODO: Submitted Or Scored
            userExam.Status = UserExamStatus.Submitted;

            decimal totalScore = 0;
            foreach (UserExamQuestion item in userExam.Questions)
            {
                bool right = false;
                decimal score = 0;
                UserExamAnswerDto? answer = input.SingleOrDefault(a => a.QuestionId == item.QuestionId);
                if (answer is null || String.IsNullOrWhiteSpace(answer.Answers))
                {
                    item.Right = right;
                    item.Score = score;
                    continue;
                }
                item.Answers = answer.Answers;

                Question question = await questionRepository.GetAsync(item.QuestionId);
                List<QuestionAnswer> questionAnswers = await questionAnswerRepository.GetListAsync(item.QuestionId);
                if ((question.QuestionType == QuestionType.SingleSelect || question.QuestionType == QuestionType.Judge)
                    && item.Answers == (questionAnswers.SingleOrDefault(a => a.Right)?.Id.ToString() ?? ""))
                {
                    totalScore += item.QuestionScore;
                    score = item.QuestionScore;
                    right = true;
                }
                else if (question.QuestionType == QuestionType.MultiSelect
                    && (new HashSet<string>(item.Answers.Split(ExamConsts.Splitter)).SetEquals(questionAnswers.Where(a => a.Right).Select(a => a.Id.ToString()))))
                {
                    totalScore += item.QuestionScore;
                    score = item.QuestionScore;
                    right = true;
                }
                else if (question.QuestionType == QuestionType.FillInTheBlanks)
                {
                    string[] allAnswers = item.Answers.Split(ExamConsts.Splitter);
                    if (allAnswers.Length == questionAnswers.Count && allAnswers.SequenceEqual(questionAnswers.Select(a => a.Content)))
                    {
                        // TODO:一空多项，多空多项，无序
                        right = true;
                        score = item.QuestionScore;
                    }
                }

                item.Right = right;
                item.Score = score;
            }

            userExam.TotalScore = totalScore;
            await UserExamRepository.UpdateAsync(userExam);
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