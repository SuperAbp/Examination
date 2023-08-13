using System;
using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.ExamManagement.Exams
{
    /// <summary>
    /// 考试
    /// </summary>
    public interface IExamRepository : IRepository<Examination, Guid>
    {
    }
}