using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.ExamManagement.UserExamQuestions
{
    public class UserExamQuestionAppService(IUserExamQuestionRepository userExamQuestionRepository)
        : ExamAppService, IUserExamQuestionAppService
    {
        public virtual async Task<UserExamQuestionDetailDto> GetAsync(Guid id)
        {
            UserExamQuestion entity = await userExamQuestionRepository.GetAsync(id);

            return ObjectMapper.Map<UserExamQuestion, UserExamQuestionDetailDto>(entity);
        }

        public virtual async Task<PagedResultDto<UserExamQuestionListDto>> GetListAsync(GetUserExamQuestionsInput input)
        {
            await NormalizeMaxResultCountAsync(input);

            List<UserExamQuestionWithDetails> entities = await userExamQuestionRepository
                .GetListAsync(input.Sorting, input.SkipCount, input.MaxResultCount, input.UserExamId);
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
                if (entity.Finished)
                {
                    dto.QuestionAnalysis = entity.QuestionAnalysis;
                }

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
                    dto.QuestionAnswers.Add(answerDto);
                }
                dtos.Add(dto);
            }
            return new PagedResultDto<UserExamQuestionListDto>(0, dtos);
        }

        public virtual async Task<GetUserExamQuestionForEditorOutput> GetEditorAsync(Guid id)
        {
            UserExamQuestion entity = await userExamQuestionRepository.GetAsync(id);

            return ObjectMapper.Map<UserExamQuestion, GetUserExamQuestionForEditorOutput>(entity);
        }

        public virtual async Task<UserExamQuestionListDto> CreateAsync(UserExamQuestionCreateDto input)
        {
            // TODO:Get question score.
            UserExamQuestion entity = new(GuidGenerator.Create(), input.UserExamId, input.QuestionId, 0)
            {
                Answers = input.Answers
            };

            entity = await userExamQuestionRepository.InsertAsync(entity);
            return ObjectMapper.Map<UserExamQuestion, UserExamQuestionListDto>(entity);
        }

        public virtual async Task<UserExamQuestionListDto> AnswerAsync(Guid id, UserExamQuestionAnswerDto input)
        {
            UserExamQuestion entity = await userExamQuestionRepository.GetAsync(id);
            entity.Answers = input.Answers;
            entity = await userExamQuestionRepository.UpdateAsync(entity);
            return ObjectMapper.Map<UserExamQuestion, UserExamQuestionListDto>(entity);
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            await userExamQuestionRepository.DeleteAsync(id);
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