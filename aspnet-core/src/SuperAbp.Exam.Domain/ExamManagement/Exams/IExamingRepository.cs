using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.ExamManagement.Exams
{
    /// <summary>
    /// 考试
    /// </summary>
    public interface IExamingRepository : IRepository<Examing, System.Guid>
    {
    }
}