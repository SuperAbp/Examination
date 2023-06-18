using System;
using System.Threading.Tasks;
using SuperAbp.Exam.Admin.QuestionManagement.QuestionAnswers;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace SuperAbp.Exam.QuestionManagement.QuestionAnswers
{
    /// <summary>
    /// 答案管理
    /// </summary>
    public interface IQuestionAnswerAppService : IApplicationService
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="questionId">题目Id</param>
        /// <returns>结果</returns>
        Task<ListResultDto<QuestionAnswerListDto>> GetListAsync(Guid questionId);
    }
}