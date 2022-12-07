using System;
using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.QuestionManagement.Questions;

/// <summary>
/// 问题
/// </summary>
public interface IQuestionRepository : IRepository<Question, Guid>
{
}