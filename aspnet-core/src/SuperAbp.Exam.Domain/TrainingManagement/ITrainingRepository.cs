using System;
using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.TrainingManagement;

public interface ITrainingRepository : IRepository<Training, Guid>
{
}