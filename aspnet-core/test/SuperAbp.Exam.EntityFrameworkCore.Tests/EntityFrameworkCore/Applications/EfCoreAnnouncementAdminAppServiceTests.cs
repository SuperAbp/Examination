using SuperAbp.Exam.Admin.Announcements;
using SuperAbp.Exam.Exams;
using Xunit;

namespace SuperAbp.Exam.EntityFrameworkCore.Applications;

[Collection(ExamTestConsts.CollectionDefinitionName)]
public class EfCoreAnnouncementAdminAppServiceTests : AnnouncementAdminAppServiceTests<ExamEntityFrameworkCoreTestModule>
{
}