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
using SuperAbp.Exam.KnowledgePoints;
using SuperAbp.Exam.MistakesReviews;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.EventBus.Local;
using SuperAbp.Exam.MistakesReviews.Events;
using static SuperAbp.Exam.ExamManagement.UserExams.UserExamDetailDto;
using static SuperAbp.Exam.ExamManagement.UserExams.UserExamDetailDto.SectionDto;
using static SuperAbp.Exam.ExamManagement.UserExams.UserExamDetailDto.SectionDto.QuestionDto;
using SuperAbp.Exam.QuestionManagement.Questions.QuestionOptions;

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

        public async Task<UserExamDetailDto?> GetUnfinishedAsync()
        {
            UserExam? userExam = await UserExamRepository.GetUnfinishedAsync(CurrentUser.GetId());
            return userExam is null ? null : ObjectMapper.Map<UserExam, UserExamDetailDto>(userExam);
        }

        public virtual async Task<UserExamDetailDto> GetAsync(Guid id)
        {
            UserExam userExam = await UserExamRepository.GetAsync(id);
            Examination exam = await examRepository.GetAsync(userExam.ExamId);
            List<Guid> questionIds = userExam.Sections.SelectMany(s => s.Questions).Select(q => q.QuestionId).ToList();
            List<Question> questions = await questionRepository.GetByIdsAsync(questionIds);
            UserExamDetailDto dto = ObjectMapper.Map<UserExam, UserExamDetailDto>(userExam);
            dto.ExamName = exam.Name;
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

            // Map sections with their questions
            var sectionDtos = new List<SectionDto>();
            var questionMap = questions.ToDictionary(q => q.Id, q => q);

            foreach (var section in userExam.Sections.OrderBy(s => s.Order))
            {
                SectionDto sectionDto = ObjectMapper.Map<UserExamSection, SectionDto>(section);

                List<QuestionDto> sectionQuestions = [];

                foreach (var userExamQuestion in section.Questions.OrderBy(q => q.Order))
                {
                    if (!questionMap.TryGetValue(userExamQuestion.QuestionId, out var question))
                        continue;

                    QuestionDto questionDto = ObjectMapper.Map<Question, QuestionDto>(question);
                    questionDto.Right = userExamQuestion.Right;
                    questionDto.Answers = userExamQuestion.Answers;
                    questionDto.QuestionScore = userExamQuestion.QuestionScore;

                    // Get knowledge points
                    List<KnowledgePoint> knowledgePoints = await questionManager.GetKnowledgePointsAsync(question.Id);
                    if (knowledgePoints.Count > 0)
                    {
                        questionDto.KnowledgePoints = knowledgePoints.Select(kp => kp.Name).ToArray();
                    }

                    // Map answers/options
                    List<OptionDto> answerDtos = [];
                    List<QuestionOption> answers = question.Options.OrderBy(a => a.Sort).ToList();
                    if (exam.RandomOrderOfOption && new List<QuestionType> { QuestionType.SingleSelect, QuestionType.MultiSelect }.Contains(question.QuestionType))
                    {
                        answers = answers.OrderBy(_ => Guid.NewGuid()).ToList();
                    }

                    foreach (QuestionOption answer in answers)
                    {
                        OptionDto optionDto = ObjectMapper.Map<QuestionOption, OptionDto>(answer);

                        if (userExam.IsSubmitted())
                        {
                            optionDto.Right = answer.Right;
                        }
                        answerDtos.Add(optionDto);
                    }
                    questionDto.Options = answerDtos;
                    sectionQuestions.Add(questionDto);
                }

                sectionDto.Questions = sectionQuestions;
                sectionDtos.Add(sectionDto);
            }

            dto.Sections = sectionDtos;
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
                UserExamId = userExam.Id,
                TenantId = CurrentTenant.Id
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

            if (!(examination.Status == ExaminationStatus.Grading ||
                  examination.Status == ExaminationStatus.Completed) &&
                examination.EndTime.HasValue &&
                examination.EndTime.Value.AddMinutes(5) < Clock.Now)
            {
                throw new InvalidExamStatusException(examination.Status);
            }
            if (userExam.Status != UserExamStatus.InProgress)
            {
                throw new InvalidUserExamStatusException(userExam.Status);
            }
            userExam.FinishedTime = clock.Now;
            userExam.Status = examination.ManualReview ? UserExamStatus.Submitted : UserExamStatus.Scored;

            decimal totalScore = 0;
            List<Task> publishEvents = [];
            foreach (UserExamQuestion item in userExam.Sections.SelectMany(s => s.Questions))
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
                    && item.Answers == (question.Options.SingleOrDefault(a => a.Right)?.Id.ToString() ?? ""))
                {
                    totalScore += item.QuestionScore;
                    score = item.QuestionScore;
                    right = true;
                }
                else if (question.QuestionType == QuestionType.MultiSelect
                    && (new HashSet<string>(item.Answers.Split(ExamConsts.Splitter)).SetEquals(question.Options.Where(a => a.Right).Select(a => a.Id.ToString()))))
                {
                    totalScore += item.QuestionScore;
                    score = item.QuestionScore;
                    right = true;
                }
                else if (question.QuestionType == QuestionType.FillInTheBlanks)
                {
                    string[] allAnswers = item.Answers.Split(ExamConsts.Splitter);
                    if (allAnswers.Length == question.Options.Count && allAnswers.SequenceEqual(question.Options.Select(a => a.Content)))
                    {
                        // TODO:一空多项，多空多项，无序
                        right = true;
                        score = item.QuestionScore;
                    }
                }

                item.Right = right;
                item.Score = score;

                publishEvents.Add(localEventBus.PublishAsync(new AnsweredQuestionEvent(
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