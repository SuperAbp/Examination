using System;
using Volo.Abp.Application.Dtos;

namespace SuperAbp.Exam.ExamManagement.UserExamQuestions
{
    /// <summary>
    /// 列表
    /// </summary>
    public class UserExamQuestionDetailDto: EntityDto<Guid>
    {
        public System.Guid QuestionId { get; set; }
        public System.Nullable<System.Boolean> Right { get; set; }
        public System.Nullable<System.Decimal> Score { get; set; }
    }
}