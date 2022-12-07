using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.Admin.QuestionManagement.QuestionAnswers
{
    /// <summary>
    /// 列表
    /// </summary>
    public class QuestionAnswerListDto: EntityDto<Guid>
    {
        public bool Right { get; set; }
        public string Content { get; set; }
        public string Analysis { get; set; }
    }
}