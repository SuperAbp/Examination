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
using SuperAbp.Exam.MistakesReviews;
using Volo.Abp;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.ObjectMapping;
using SuperAbp.Exam.QuestionManagement.Questions.QuestionAnswers;
using Volo.Abp.EventBus.Local;
using SuperAbp.Exam.MistakesReviews.Events;

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
        IMistakesReviewRepository mistakesReviewRepository,
        IBackgroundJobManager backgroundJobManager,
        ILocalEventBus localEventBus)
        : ExamAppService, IUserExamAppService
    {
        protected IUserExamRepository UserExamRepository { get; } = userExamRepository;
        protected IExamRepository ExamRepository { get; } = examRepository;
        protected IBackgroundJobManager BackgroundJobManager { get; } = backgroundJobManager;
        private readonly ILocalEventBus _localEventBus;

        public async Task<UserExamDetailDto?> GetUnfinishedAsync()
        {
            UserExam? userExam = await UserExamRepository.GetUnfinishedAsync(CurrentUser.GetId());
            return userExam is null ? null : ObjectMapper.Map<UserExam, UserExamDetailDto>(userExam);
        }

        public virtual async Task<UserExamDetailDto> GetAsync(Guid id)
        {
            UserExam userExam = await UserExamRepository.GetAsync(id);
            Examination exam = await examRepository.GetAsync(userExam.ExamId);
            List<Guid> questionIds = userExam.Questions.Select(q => q.QuestionId).ToList();
            List<Question> questions = await questionRepository.GetByIdsAsync(questionIds);
            UserExamDetailDto dto = ObjectMapper.Map<UserExam, UserExamDetailDto>(userExam);
            dto.AnswerMode = exam.AnswerMode;
            if (userExam.StartTime.HasValue)
            {
                DateTime endTime = userExam.StartTime.Value.AddMinutes(exam.TotalTime);
                if (exam.EndTime.HasValue && endTime > exam.EndTime)
                {
                    endTime = exam.EndTime.Value;
                }
                dto.EndTime = endTime;
            }
            else
            {
                userExam.StartTime = clock.Now;
                await UserExamRepository.UpdateAsync(userExam);
            }

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
                List<QuestionAnswer> answers = question.Answers;
                if (exam.RandomOrderOfOption)
                {
                    answers = answers.OrderBy(_ => Guid.NewGuid()).ToList();
                }
                foreach (QuestionAnswer answer in answers)
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
            userExam.StartTime = Clock.Now;
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
            List<Task> publishEvents = [];
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
                if ((question.QuestionType == QuestionType.SingleSelect || question.QuestionType == QuestionType.Judge)
                    && item.Answers == (question.Answers.SingleOrDefault(a => a.Right)?.Id.ToString() ?? ""))
                {
                    totalScore += item.QuestionScore;
                    score = item.QuestionScore;
                    right = true;
                }
                else if (question.QuestionType == QuestionType.MultiSelect
                    && (new HashSet<string>(item.Answers.Split(ExamConsts.Splitter)).SetEquals(question.Answers.Where(a => a.Right).Select(a => a.Id.ToString()))))
                {
                    totalScore += item.QuestionScore;
                    score = item.QuestionScore;
                    right = true;
                }
                else if (question.QuestionType == QuestionType.FillInTheBlanks)
                {
                    string[] allAnswers = item.Answers.Split(ExamConsts.Splitter);
                    if (allAnswers.Length == question.Answers.Count && allAnswers.SequenceEqual(question.Answers.Select(a => a.Content)))
                    {
                        // TODO:一空多项，多空多项，无序
                        right = true;
                        score = item.QuestionScore;
                    }
                }

                item.Right = right;
                item.Score = score;

                publishEvents.Add(_localEventBus.PublishAsync(new AnsweredQuestionEvent(
                    item.QuestionId,
                    userExam.UserId,
                    right
                )));
            }

            await Task.WhenAll(publishEvents);

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