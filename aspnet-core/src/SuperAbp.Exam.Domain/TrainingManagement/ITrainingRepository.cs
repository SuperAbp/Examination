using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.TrainingManagement;

public interface ITrainingRepository : IRepository<Training, Guid>
{
    Task<bool> AnyQuestionAsync(TrainingSource trainingSource, Guid questionId, CancellationToken cancellationToken = default);

    Task<List<Training>> GetListAsync(
        string? sorting = null,
        int skipCount = 0,
        int maxResultCount = int.MaxValue,
        int? trainingSource = null,
        Guid? questionRepositoryId = null,
        Guid? userId = null,
        CancellationToken cancellationToken = default);
}