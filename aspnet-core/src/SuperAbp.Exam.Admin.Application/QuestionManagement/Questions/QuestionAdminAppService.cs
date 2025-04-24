using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using SuperAbp.Exam.QuestionManagement.Questions;
using SuperAbp.Exam.Permissions;
using SuperAbp.Exam.QuestionManagement.QuestionAnswers;
using SuperAbp.Exam.QuestionManagement.QuestionBanks;
using SuperAbp.Exam.QuestionManagement.QuestionKnowledgePoints;

namespace SuperAbp.Exam.Admin.QuestionManagement.Questions
{
    [Authorize(ExamPermissions.Questions.Default)]
    public class QuestionAdminAppService(
        QuestionManager questionManager,
        QuestionAnswerManager questionAnswerManager,
        IQuestionRepository questionRepository,
        IQuestionBankRepository questionBankRepository,
        IQuestionAnswerRepository questionAnswerRepository,
        IQuestionKnowledgePointRepository questionKnowledgePointRepository,
        Func<int, IQuestionAnalysis> questionAnalysis)
        : ExamAppService, IQuestionAdminAppService
    {
        public virtual async Task<PagedResultDto<QuestionListDto>> GetListAsync(GetQuestionsInput input)
        {
            await NormalizeMaxResultCountAsync(input);

            var questionQueryable = await questionRepository.GetQueryableAsync();

            questionQueryable = questionQueryable
                .WhereIf(input.QuestionBankIds.Length > 0, q => input.QuestionBankIds.Contains(q.QuestionBankId))
                .WhereIf(input.QuestionType.HasValue, q => q.QuestionType == input.QuestionType.Value)
                .WhereIf(!input.Content.IsNullOrWhiteSpace(), q => q.Content.Contains(input.Content));

            var queryable = from q in questionQueryable
                            join r in (await questionBankRepository.GetQueryableAsync()) on q.QuestionBankId equals r.Id
                            select new QuestionBankWithDetails
                            {
                                Id = q.Id,
                                QuestionBank = r.Title,
                                Analysis = q.Analysis,
                                Content = q.Content,
                                QuestionType = q.QuestionType,
                                CreationTime = q.CreationTime
                            };

            long totalCount = await AsyncExecuter.CountAsync(queryable);

            var entities = await AsyncExecuter.ToListAsync(queryable
                .OrderBy(input.Sorting ?? QuestionConsts.DefaultSorting)
                .PageBy(input));

            var dtos = ObjectMapper.Map<List<QuestionBankWithDetails>, List<QuestionListDto>>(entities);

            return new PagedResultDto<QuestionListDto>(totalCount, dtos);
        }

        public virtual async Task<GetQuestionForEditorOutput> GetEditorAsync(Guid id)
        {
            Question entity = await questionRepository.GetAsync(id);
            var dto = ObjectMapper.Map<Question, GetQuestionForEditorOutput>(entity);
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
            List<QuestionAnswer> answers = [];
            foreach (QuestionImportModel item in items)
            {
                Question question = await questionManager.CreateAsync(input.QuestionBankId, QuestionType.FromValue(input.QuestionType), item.Title);
                question.Analysis = item.Analysis;

                for (int i = 0; i < item.Options.Count; i++)
                {
                    QuestionImportModel.Option option = item.Options[i];
                    QuestionAnswer questionAnswer =
                        await questionAnswerManager.CreateAsync(question.Id, option.Content, item.Answers.Contains(i));
                    questionAnswer.Analysis = option.Analysis;

                    answers.Add(questionAnswer);
                }

                questions.Add(question);
            }
            await questionRepository.InsertManyAsync(questions);
            await questionAnswerRepository.InsertManyAsync(answers);
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
            await CreateOrUpdateAnswerAsync(question.Id, input.Options);
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
            await CreateOrUpdateAnswerAsync(question.Id, input.Options);
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
                throw new QuestionAnswerCorrectCountErrorException();
            }
        }

        protected virtual async Task CreateOrUpdateAnswerAsync(Guid questionId, QuestionCreateOrUpdateAnswerDto[] answers)
        {
            List<QuestionAnswer> questionAnswers = await questionAnswerRepository.GetListAsync(questionId);
            List<QuestionAnswer> newQuestionAnswers = [];
            List<QuestionAnswer> updateQuestionAnswers = [];
            foreach (QuestionCreateOrUpdateAnswerDto answer in answers)
            {
                if (answer.Id.HasValue)
                {
                    QuestionAnswer questionAnswer = questionAnswers.Single(a => a.Id == answer.Id.Value);
                    await questionAnswerManager.SetContentAsync(questionAnswer, answer.Content);
                    questionAnswer.Right = answer.Right;
                    questionAnswer.Analysis = answer.Analysis;
                    questionAnswer.Sort = answer.Sort;
                    updateQuestionAnswers.Add(questionAnswer);
                }
                else
                {
                    QuestionAnswer questionAnswer = await questionAnswerManager.CreateAsync(questionId, answer.Content, answer.Right);
                    questionAnswer.Sort = answer.Sort;
                    questionAnswer.Analysis = answer.Analysis;
                    questionAnswers.Add(questionAnswer);
                    newQuestionAnswers.Add(questionAnswer);
                }
            }
            await questionAnswerRepository.InsertManyAsync(newQuestionAnswers);
            await questionAnswerRepository.UpdateManyAsync(updateQuestionAnswers);
        }

        [Authorize(ExamPermissions.Questions.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await questionAnswerRepository.DeleteByQuestionIdAsync(id);
            await questionRepository.DeleteAsync(id);
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