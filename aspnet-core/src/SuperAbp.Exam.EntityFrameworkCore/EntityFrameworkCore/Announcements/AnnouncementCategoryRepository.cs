using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SuperAbp.Exam.Announcements;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace SuperAbp.Exam.EntityFrameworkCore.Announcements;

public class AnnouncementCategoryRepository : EfCoreRepository<ExamDbContext, AnnouncementCategory, Guid>, IAnnouncementCategoryRepository
{
    public AnnouncementCategoryRepository(IDbContextProvider<ExamDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}