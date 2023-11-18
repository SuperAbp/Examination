using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.ExamManagement.UserExamQuestions
{
    /// <summary>
    /// 用户考题管理
    /// </summary>
    public class UserExamQuestionAppService : ExamAppService, IUserExamQuestionAppService
    {
        private readonly IUserExamQuestionRepository _userExamQuestionRepository;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="userExamQuestionRepository"></param>
        public UserExamQuestionAppService(
            IUserExamQuestionRepository userExamQuestionRepository)
        {
            _userExamQuestionRepository = userExamQuestionRepository;
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public virtual async Task<UserExamQuestionDetailDto> GetAsync(Guid id)
        {
            UserExamQuestion entity = await _userExamQuestionRepository.GetAsync(id);

            return ObjectMapper.Map<UserExamQuestion, UserExamQuestionDetailDto>(entity);
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="input">查询条件</param>
        /// <returns>结果</returns>
        public virtual async Task<PagedResultDto<UserExamQuestionListDto>> GetListAsync(GetUserExamQuestionsInput input)
        {
            await NormalizeMaxResultCountAsync(input);

            var entities = await _userExamQuestionRepository.GetListAsync(input.Sorting, input.SkipCount,
                input.MaxResultCount, input.UserExamId);
            var dtos = entities.Select(q => new UserExamQuestionListDto()
            {
                Id = q.Id,
                Answers = q.Answers,
                QuestionId = q.QuestionId,
                Question = q.Question,
                QuestionScore = q.QuestionScore,
                QuestionType = q.QuestionType,
                QuestionAnswers = q.QuestionAnswers.Select(qa => new UserExamQuestionListDto.QuestionAnswerListDto
                {
                    Id = qa.Id,
                    Content = qa.Content
                }).ToList()
            }).ToList();
            return new PagedResultDto<UserExamQuestionListDto>(0, dtos);
        }

        /// <summary>
        /// 获取修改
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public virtual async Task<GetUserExamQuestionForEditorOutput> GetEditorAsync(Guid id)
        {
            UserExamQuestion entity = await _userExamQuestionRepository.GetAsync(id);

            return ObjectMapper.Map<UserExamQuestion, GetUserExamQuestionForEditorOutput>(entity);
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<UserExamQuestionListDto> CreateAsync(UserExamQuestionCreateDto input)
        {
            var entity = ObjectMapper.Map<UserExamQuestionCreateDto, UserExamQuestion>(input);
            entity = await _userExamQuestionRepository.InsertAsync(entity, true);
            return ObjectMapper.Map<UserExamQuestion, UserExamQuestionListDto>(entity);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<UserExamQuestionListDto> AnswerAsync(Guid id, UserExamQuestionAnswerDto input)
        {
            UserExamQuestion entity = await _userExamQuestionRepository.GetAsync(id);
            entity.Answers = input.Answers;
            entity = await _userExamQuestionRepository.UpdateAsync(entity);
            return ObjectMapper.Map<UserExamQuestion, UserExamQuestionListDto>(entity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(Guid id)
        {
            await _userExamQuestionRepository.DeleteAsync(s => s.Id == id);
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