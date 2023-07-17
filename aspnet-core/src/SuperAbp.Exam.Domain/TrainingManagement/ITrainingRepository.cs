using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.TrainingManagement;

public interface ITrainingRepository : IRepository<Training, Guid>
{
    Task<bool> AnyQuestionAsync(Guid questionId);
}