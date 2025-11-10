using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.ExamManagement.UserExamQuestions
{
    /// <summary>
    /// 用户考题
    /// </summary>
    public interface IUserExamQuestionRepository : IRepository<UserExamQuestion, Guid>
    {
    }
}