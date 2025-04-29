using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SuperAbp.Exam.KnowledgePoints;
using SuperAbp.Exam.QuestionManagement.Questions;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.ExamManagement.UserExamQuestions
{
    public class UserExamQuestionAppService(IUserExamQuestionRepository userExamQuestionRepository, QuestionManager questionManager)
        : ExamAppService, IUserExamQuestionAppService
    {
        protected IUserExamQuestionRepository UserExamQuestionRepository { get; } = userExamQuestionRepository;
        protected QuestionManager QuestionManager { get; } = questionManager;

        public virtual async Task<UserExamQuestionDetailDto> GetAsync(Guid id)
        {
            UserExamQuestion entity = await UserExamQuestionRepository.GetAsync(id);

            return ObjectMapper.Map<UserExamQuestion, UserExamQuestionDetailDto>(entity);
        }

        public virtual async Task<PagedResultDto<UserExamQuestionListDto>> GetListAsync(GetUserExamQuestionsInput input)
        {
            await NormalizeMaxResultCountAsync(input);

            List<UserExamQuestionWithDetails> entities = await UserExamQuestionRepository
                .GetListAsync(input.UserExamId, input.Sorting, input.SkipCount, input.MaxResultCount);
            List<UserExamQuestionListDto> dtos = [];
            foreach (UserExamQuestionWithDetails entity in entities)
            {
                UserExamQuestionListDto dto = new()
                {
                    Id = entity.Id,
                    Answers = entity.Answers ?? String.Empty,
                    QuestionId = entity.QuestionId,
                    Question = entity.Question,
                    QuestionScore = entity.QuestionScore,
                    QuestionType = entity.QuestionType
                };
                List<KnowledgePoint> knowledgePoints = await QuestionManager.GetKnowledgePointsAsync(entity.QuestionId);
                if (knowledgePoints.Count > 0)
                {
                    dto.KnowledgePoints = knowledgePoints.Select(kp => kp.Name).ToArray();
                }
                if (entity.Finished)
                {
                    dto.QuestionAnalysis = entity.QuestionAnalysis;
                    dto.Right = entity.Right;
                    dto.Score = entity.Score;
                }

                List<UserExamQuestionListDto.QuestionAnswerListDto> answerDtos = [];
                if (entity.QuestionAnswers.Count > 0)
                {
                    foreach (UserExamQuestionWithDetails.QuestionAnswer answer in entity.QuestionAnswers)
                    {
                        UserExamQuestionListDto.QuestionAnswerListDto answerDto = new()
                        {
                            Id = answer.Id,
                            Content = answer.Content,
                        };
                        if (entity.Finished)
                        {
                            answerDto.Right = answer.Right;
                        }

                        answerDtos.Add(answerDto);
                    }

                    dto.QuestionAnswers = answerDtos;
                }

                dtos.Add(dto);
            }
            return new PagedResultDto<UserExamQuestionListDto>(0, dtos);
        }

        public virtual async Task<GetUserExamQuestionForEditorOutput> GetEditorAsync(Guid id)
        {
            UserExamQuestion entity = await UserExamQuestionRepository.GetAsync(id);

            return ObjectMapper.Map<UserExamQuestion, GetUserExamQuestionForEditorOutput>(entity);
        }

        public virtual async Task<UserExamQuestionListDto> CreateAsync(UserExamQuestionCreateDto input)
        {
            // TODO:Get question score.
            UserExamQuestion entity = new(GuidGenerator.Create(), input.UserExamId, input.QuestionId, 0)
            {
                Answers = input.Answers
            };

            entity = await UserExamQuestionRepository.InsertAsync(entity);
            return ObjectMapper.Map<UserExamQuestion, UserExamQuestionListDto>(entity);
        }

        public virtual async Task<UserExamQuestionListDto> AnswerAsync(Guid id, UserExamQuestionAnswerDto input)
        {
            UserExamQuestion entity = await UserExamQuestionRepository.GetAsync(id);
            entity.Answers = input.Answers;
            entity = await UserExamQuestionRepository.UpdateAsync(entity);
            return ObjectMapper.Map<UserExamQuestion, UserExamQuestionListDto>(entity);
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            await UserExamQuestionRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 规范最大记录数
        /// </summary>
        /// <param name="input">参数</param>
        /// <returns></returns>
        private async Task NormalizeMaxResultCountAsync(PagedAndSortedResultRequestDto input)
        {
            var maxPageSize = (await SettingProvider.GetOrNullAsync(UserExamQuestionSettings.MaxPageSize))?.To<int>();
            if (maxPageSize.HasValue && input.MaxResultCount > maxPageSize.Value)
            {
                input.MaxResultCount = maxPageSize.Value;
            }
        }
    }
}