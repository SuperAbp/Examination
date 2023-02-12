using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.PaperManagement.Papers;

/// <summary>
/// 考试
/// </summary>
public interface IPaperRepository : IRepository<Paper, Guid>
{
    /// <summary>
    /// 是否存在
    /// </summary>
    /// <param name="name">名称</param>
    /// <returns></returns>
    Task<bool> ExistsByNameAsync(string name);
}