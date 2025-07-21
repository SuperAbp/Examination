using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using SuperAbp.Exam.QuestionManagement.Questions;
using SuperAbp.Exam.Permissions;
using SuperAbp.Exam.QuestionManagement.QuestionAnswers;
using SuperAbp.Exam.QuestionManagement.Questions.QuestionAnswers;
using Volo.Abp;

namespace SuperAbp.Exam.Admin.QuestionManagement.Questions
{
    [Authorize(ExamPermissions.Questions.Default)]
    public class QuestionAdminAppService(
        QuestionManager questionManager,
        IQuestionRepository questionRepository,
        Func<int, IQuestionAnalysis> questionAnalysis)
        : ExamAppService, IQuestionAdminAppService
    {
        public virtual async Task<PagedResultDto<QuestionListDto>> GetListAsync(GetQuestionsInput input)
        {
            await NormalizeMaxResultCountAsync(input);

            int totalCount = await questionRepository.GetCountAsync(input.Content, input.QuestionType, input.QuestionBankIds.ToList());
            List<QuestionWithDetails> questions = await questionRepository.GetListAsync(input.Sorting, input.SkipCount, input.MaxResultCount,
                input.Content, input.QuestionType, input.QuestionBankIds.ToList());

            var dtos = ObjectMapper.Map<List<QuestionWithDetails>, List<QuestionListDto>>(questions);

            return new PagedResultDto<QuestionListDto>(totalCount, dtos);
        }

        public virtual async Task<GetQuestionForEditorOutput> GetEditorAsync(Guid id)
        {
            Question entity = await questionRepository.GetAsync(id);
            var dto = ObjectMapper.Map<Question, GetQuestionForEditorOutput>(entity);
            dto.Answers = ObjectMapper.Map<List<QuestionAnswer>, List<QuestionAnswerDto>>(entity.Answers);
            List<Guid> points = await questionManager.GetKnowledgePointIdsAsync(id);
            if (points.Count > 0)
            {
                dto.KnowledgePointIds = points.ToArray();
            }

            return dto;
        }

        [Authorize(ExamPermissions.Questions.Import)]
        public virtual async Task ImportAsync(QuestionImportDto input)
        {
            string[] lines = input.Content.Split(["\r\n", "\r", "\n"], StringSplitOptions.None);
            List<QuestionImportModel> items = questionAnalysis(input.QuestionType).Analyse(lines);
            List<Question> questions = [];
            foreach (QuestionImportModel item in items)
            {
                Question question = await questionManager.CreateAsync(input.QuestionBankId, QuestionType.FromValue(input.QuestionType), item.Title);
                question.Analysis = item.Analysis;

                for (int i = 0; i < item.Options.Count; i++)
                {
                    QuestionImportModel.Option option = item.Options[i];

                    question.AddAnswer(GuidGenerator.Create(), option.Content, item.Answers.Contains(i), 0, option.Analysis);
                }

                questions.Add(question);
            }
            await questionRepository.InsertManyAsync(questions);
        }

        [Authorize(ExamPermissions.Questions.Create)]
        public virtual async Task<QuestionListDto> CreateAsync(QuestionCreateDto input)
        {
            ValidationCorrectCountAsync(input.QuestionType, input.Options.Count(a => a.Right));
            Question question = await questionManager.CreateAsync(input.QuestionBankId, QuestionType.FromValue(input.QuestionType), input.Content);
            question.Analysis = input.Analysis;
            question = await questionRepository.InsertAsync(question);
            if (input.KnowledgePointIds is not null)
            {
                await questionManager.SetKnowledgePointAsync(question, input.KnowledgePointIds);
            }
            CreateOrUpdateAnswer(question, input.Options);
            return ObjectMapper.Map<Question, QuestionListDto>(question);
        }

        [Authorize(ExamPermissions.Questions.Update)]
        public virtual async Task<QuestionListDto> UpdateAsync(Guid id, QuestionUpdateDto input)
        {
            Question question = await questionRepository.GetAsync(id);
            ValidationCorrectCountAsync(question.QuestionType.Value, input.Options.Count(a => a.Right));

            await questionManager.SetContentAsync(question, input.Content);
            question.Analysis = input.Analysis;
            question.QuestionBankId = input.QuestionBankId;
            question = await questionRepository.UpdateAsync(question);
            if (input.KnowledgePointIds is not null)
            {
                await questionManager.SetKnowledgePointAsync(question, input.KnowledgePointIds);
            }
            CreateOrUpdateAnswer(question, input.Options);
            return ObjectMapper.Map<Question, QuestionListDto>(question);
        }

        private static void ValidationCorrectCountAsync(int questionType, int count)
        {
            if (!(QuestionType.FromValue(questionType).Name switch
            {
                nameof(QuestionType.Judge) => count == 1,
                nameof(QuestionType.SingleSelect) => count == 1,
                nameof(QuestionType.MultiSelect) => count > 1,
                _ => true
            }))
            {
                throw new BusinessException(ExamDomainErrorCodes.Questions.CorrectCountError);
            }
        }

        protected virtual void CreateOrUpdateAnswer(Question question, QuestionCreateOrUpdateAnswerDto[] answers)
        {
            foreach (QuestionCreateOrUpdateAnswerDto answer in answers)
            {
                if (answer.Id.HasValue)
                {
                    question.UpdateAnswer(answer.Id.Value, answer.Content, answer.Right, answer.Sort, answer.Analysis);
                }
                else
                {
                    question.AddAnswer(GuidGenerator.Create(), answer.Content, answer.Right, answer.Sort, answer.Analysis);
                }
            }
        }

        [Authorize(ExamPermissions.Questions.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await questionRepository.DeleteAsync(id);
        }

        [Authorize(ExamPermissions.Questions.Delete)]
        public virtual async Task DeleteAnswerAsync(Guid id, Guid answerId)
        {
            Question question = await questionRepository.GetAsync(id);
            question.RemoveAnswer(answerId);
            await questionRepository.UpdateAsync(question);
        }

        /// <summary>
        /// 规范最大记录数
        /// </summary>
        /// <param name="input">参数</param>
        /// <returns></returns>
        private async Task NormalizeMaxResultCountAsync(PagedAndSortedResultRequestDto input)
        {
            var maxPageSize = (await SettingProvider.GetOrNullAsync(QuestionSettings.MaxPageSize))?.To<int>();
            if (maxPageSize.HasValue && input.MaxResultCount > maxPageSize.Value)
            {
                input.MaxResultCount = maxPageSize.Value;
            }
        }
    }
}