using Microsoft.AspNetCore.Authorization;
using SuperAbp.Exam.ExamManagement.Exams;
using SuperAbp.Exam.QuestionManagement.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SuperAbp.Exam.ExamManagement.UserExamQuestions;
using SuperAbp.Exam.QuestionManagement.QuestionAnswers;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Timing;
using Volo.Abp.Users;
using static SuperAbp.Exam.ExamDomainErrorCodes;
using static SuperAbp.Exam.ExamManagement.UserExams.UserExamDetailDto.QuestionDto;
using Volo.Abp.Domain.Entities;
using SuperAbp.Exam.KnowledgePoints;

namespace SuperAbp.Exam.ExamManagement.UserExams
{
    [Authorize]
    public class UserExamAppService(
        IClock clock,
        IUserExamRepository userExamRepository,
        UserExamManager userExamManager,
        IExamRepository examRepository,
        IKnowledgePointRepository knowledgePointRepository,
        IQuestionRepository questionRepository, QuestionManager questionManager,
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
            UserExam userExam = await userExamRepository.GetAsync(id);
            List<Guid> questionIds = userExam.Questions.Select(q => q.QuestionId).ToList();
            List<Question> questions = await questionRepository.GetByIdsAsync(questionIds);
            var dto = ObjectMapper.Map<UserExam, UserExamDetailDto>(userExam);
            List<UserExamDetailDto.QuestionDto> questionDtos = [];
            foreach (Question question in questions)
            {
                var questionDto = ObjectMapper.Map<Question, UserExamDetailDto.QuestionDto>(question);
                questionDto.Right = userExam.Questions.First(q => q.QuestionId == question.Id).Right;
                questionDto.Answers = userExam.Questions.FirstOrDefault(q => q.QuestionId == question.Id)?.Answers;
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
                    if (userExam.Finished)
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

            int totalCount = await userExamRepository.GetCountAsync(CurrentUser.GetId());
            List<UserExamWithDetails> entities = await userExamRepository.GetListWithDetailAsync(
                input.Sorting ?? UserExamConsts.DefaultSorting, input.SkipCount, input.MaxResultCount,
                CurrentUser.GetId());

            List<UserExamListDto> dtos = ObjectMapper.Map<List<UserExamWithDetails>, List<UserExamListDto>>(entities);
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
            userExam.FinishedTime = clock.Now;
            await userExamRepository.UpdateAsync(userExam);

            List<UserExamQuestionWithDetails> userExamQuestions = await userExamQuestionRepository.GetListAsync(userExamId: id);
            List<UserExamQuestion> questions = [];
            decimal totalScore = 0;
            foreach (UserExamQuestionWithDetails item in userExamQuestions)
            {
                if (item.Answers is null)
                {
                    continue;
                }

                bool right = false;
                decimal score = 0;
                // TODO:更新UserExamQuestion的Right和Score
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
                    // TODO:一空多项，多空多项，无序
                }

                UserExamQuestion userExamQuestion = await userExamQuestionRepository.GetAsync(item.Id);
                userExamQuestion.Right = right;
                userExamQuestion.Score = score;
                questions.Add(userExamQuestion);
            }

            await userExamQuestionRepository.UpdateManyAsync(questions);

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