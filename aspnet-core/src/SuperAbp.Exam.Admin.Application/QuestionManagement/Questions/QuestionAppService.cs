using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Linq;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using SuperAbp.Exam.QuestionManagement.Questions;
using SuperAbp.Exam.Permissions;
using SuperAbp.Exam.Admin.QuestionManagement.Questions;
using SuperAbp.Exam.QuestionManagement.QuestionRepos;
using static SuperAbp.Exam.Permissions.ExamPermissions;
using System.Text.RegularExpressions;
using SuperAbp.Exam.ExamManagement.UserExamQuestions;
using SuperAbp.Exam.QuestionManagement.QuestionAnswers;

namespace SuperAbp.Exam.Admin.QuestionManagement.Questions
{
    /// <summary>
    /// 问题管理
    /// </summary>
    [Authorize(ExamPermissions.Questions.Default)]
    public class QuestionAppService(
        IQuestionRepository questionRepository,
        IQuestionRepoRepository questionRepoRepository,
        IQuestionAnswerRepository questionAnswerRepository,
        Func<QuestionType, IQuestionAnalysis> questionAnalysis)
        : ExamAppService, IQuestionAppService
    {
        public virtual async Task<PagedResultDto<QuestionListDto>> GetListAsync(GetQuestionsInput input)
        {
            await NormalizeMaxResultCountAsync(input);

            var questionQueryable = await questionRepository.GetQueryableAsync();

            questionQueryable = questionQueryable
                .WhereIf(input.QuestionRepositoryIds.Length > 0, q => input.QuestionRepositoryIds.Contains(q.QuestionRepositoryId))
                .WhereIf(input.QuestionType.HasValue, q => q.QuestionType == input.QuestionType.Value)
                .WhereIf(!input.Content.IsNullOrWhiteSpace(), q => q.Content.Contains(input.Content));

            var queryable = from q in questionQueryable
                            join r in (await questionRepoRepository.GetQueryableAsync()) on q.QuestionRepositoryId equals r.Id
                            select new QuestionRepositoryDetail
                            {
                                Id = q.Id,
                                QuestionRepository = r.Title,
                                Analysis = q.Analysis,
                                Content = q.Content,
                                QuestionType = q.QuestionType,
                                CreationTime = q.CreationTime
                            };

            long totalCount = await AsyncExecuter.CountAsync(queryable);

            var entities = await AsyncExecuter.ToListAsync(queryable
                .OrderBy(input.Sorting ?? QuestionConsts.DefaultSorting)
                .PageBy(input));

            var dtos = ObjectMapper.Map<List<QuestionRepositoryDetail>, List<QuestionListDto>>(entities);

            return new PagedResultDto<QuestionListDto>(totalCount, dtos);
        }

        public virtual async Task<GetQuestionForEditorOutput> GetEditorAsync(Guid id)
        {
            Question entity = await questionRepository.GetAsync(id);

            return ObjectMapper.Map<Question, GetQuestionForEditorOutput>(entity);
        }

        public virtual async Task ImportAsync(QuestionImportDto input)
        {
            string[] lines = input.Content.Split(["\r\n"], StringSplitOptions.None);
            List<QuestionImportModel> items = questionAnalysis(input.QuestionType).Analyse(lines);
            List<Question> questions = [];
            List<QuestionAnswer> answers = [];
            foreach (QuestionImportModel item in items)
            {
                Question question = new Question(GuidGenerator.Create())
                {
                    Analysis = item.Analysis,
                    Content = item.Title,
                    QuestionRepositoryId = input.QuestionRepositoryId,
                    QuestionType = input.QuestionType
                };
                for (int i = 0; i < item.Options.Count; i++)
                {
                    QuestionImportModel.Option option = item.Options[i];
                    QuestionAnswer questionAnswer = new QuestionAnswer(GuidGenerator.Create())
                    {
                        Content = option.Content,
                        Analysis = option.Analysis,
                        QuestionId = question.Id,
                        Right = item.Answers.Contains(i)
                    };
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
            var entity = ObjectMapper.Map<QuestionCreateDto, Question>(input);
            entity = await questionRepository.InsertAsync(entity, true);
            return ObjectMapper.Map<Question, QuestionListDto>(entity);
        }

        [Authorize(ExamPermissions.Questions.Update)]
        public virtual async Task<QuestionListDto> UpdateAsync(Guid id, QuestionUpdateDto input)
        {
            Question entity = await questionRepository.GetAsync(id);
            entity = ObjectMapper.Map(input, entity);
            entity = await questionRepository.UpdateAsync(entity);
            return ObjectMapper.Map<Question, QuestionListDto>(entity);
        }

        [Authorize(ExamPermissions.Questions.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await questionRepository.DeleteAsync(s => s.Id == id);
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