using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SuperAbp.Exam.Permissions;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.QuestionManagement.QuestionAnswers
{
    /// <summary>
    /// 答案管理
    /// </summary>
    [Authorize(ExamPermissions.QuestionAnswers.Default)]
    public class QuestionAnswerAppService : ExamAppService, IQuestionAnswerAppService
    {
        private readonly IQuestionAnswerRepository _questionAnswerRepository;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="questionAnswerRepository"></param>
        public QuestionAnswerAppService(
            IQuestionAnswerRepository questionAnswerRepository)
        {
            _questionAnswerRepository = questionAnswerRepository;
        }

        public virtual async Task<ListResultDto<QuestionAnswerListDto>> GetListAsync(Guid questionId)
        {
            var answers = await _questionAnswerRepository.GetListAsync(questionId);

            var dtos = ObjectMapper.Map<List<QuestionAnswer>, List<QuestionAnswerListDto>>(answers);

            return new ListResultDto<QuestionAnswerListDto>(dtos);
        }
    }
}