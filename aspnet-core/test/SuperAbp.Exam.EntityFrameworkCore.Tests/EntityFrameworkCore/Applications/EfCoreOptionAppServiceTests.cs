using Xunit;

namespace SuperAbp.Exam.EntityFrameworkCore.Applications;

[Collection(ExamTestConsts.CollectionDefinitionName)]
public class EfCoreOptionAppServiceTests : OptionAppServiceTests<ExamEntityFrameworkCoreTestModule>
{
}