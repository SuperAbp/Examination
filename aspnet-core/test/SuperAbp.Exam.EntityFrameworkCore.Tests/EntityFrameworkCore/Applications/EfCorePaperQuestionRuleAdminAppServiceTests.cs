using SuperAbp.Exam.Papers;
using Xunit;

namespace SuperAbp.Exam.EntityFrameworkCore.Applications;

[Collection(ExamTestConsts.CollectionDefinitionName)]
public class EfCorePaperQuestionRuleAdminAppServiceTests : PaperQuestionRuleAdminAppServiceTests<ExamEntityFrameworkCoreTestModule>
{
}