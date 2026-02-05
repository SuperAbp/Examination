using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace SuperAbp.Exam.Announcements;

public interface IAnnouncementCategoryRepository : IRepository<AnnouncementCategory, Guid>
{
}