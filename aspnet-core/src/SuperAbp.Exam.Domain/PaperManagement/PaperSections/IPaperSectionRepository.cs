using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.PaperManagement.PaperSections
{
    /// <summary>
    /// 试卷大题
    /// </summary>
    public interface IPaperSectionRepository : IRepository<PaperSection, Guid>
    {
        Task<List<PaperSection>> GetListByPaperIdAsync(Guid paperId, CancellationToken cancellationToken = default);

        Task<PaperSection> GetByPaperIdAndOrderAsync(Guid paperId, int order, CancellationToken cancellationToken = default);
    }
}
