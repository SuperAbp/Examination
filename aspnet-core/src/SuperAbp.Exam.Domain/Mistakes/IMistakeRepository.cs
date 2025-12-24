using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using SuperAbp.Exam.QuestionManagement.Questions;

namespace SuperAbp.Exam.Mistakes;

public interface IMistakeRepository : IRepository<Mistake, Guid>
{
    Task<List<MistakeWithDetails>> GetListAsync(string? sorting = null, int skipCount = 0, int maxResultCount = int.MaxValue,
        Guid? userId = null, string? questionContent = null, QuestionType? questionType = null, CancellationToken cancellationToken = default);

    Task<long> CountAsync(Guid? userId, string? questionContent = null, QuestionType? questionType = null, CancellationToken cancellationToken = default);
}