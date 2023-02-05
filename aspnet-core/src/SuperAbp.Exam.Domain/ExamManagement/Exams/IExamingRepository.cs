using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.ExamManagement.Exams
{
    /// <summary>
    /// 考试
    /// </summary>
    public interface IExamingRepository : IRepository<Examing, Guid>
    {
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        Task<bool> ExistsByNameAsync(string name);
    }
}