using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.ExamManagement.Exams
{
    /// <summary>
    /// 考试
    /// </summary>
    public interface IExamRepository : IRepository<Examination, Guid>
    {
        Task<bool> ExistsByPaperIdAsync(Guid paperId, CancellationToken cancellationToken = default);
    }
}