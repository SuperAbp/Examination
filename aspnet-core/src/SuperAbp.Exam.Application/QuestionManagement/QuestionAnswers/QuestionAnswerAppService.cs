using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SuperAbp.Exam.Permissions;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.QuestionManagement.QuestionAnswers
{
    [Authorize(ExamPermissions.QuestionAnswers.Default)]
    public class QuestionAnswerAppService(IQuestionAnswerRepository questionAnswerRepository)
        : ExamAppService, IQuestionAnswerAppService
    {
        public virtual async Task<ListResultDto<QuestionAnswerListDto>> GetListAsync(Guid questionId)
        {
            List<QuestionAnswer> answers = await questionAnswerRepository.GetListAsync(questionId);
            return new ListResultDto<QuestionAnswerListDto>(ObjectMapper.Map<List<QuestionAnswer>, List<QuestionAnswerListDto>>(answers));
        }
    }
}