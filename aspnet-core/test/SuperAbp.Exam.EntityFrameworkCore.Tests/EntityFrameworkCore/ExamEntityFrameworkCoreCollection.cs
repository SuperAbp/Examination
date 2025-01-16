using Xunit;

namespace SuperAbp.Exam.EntityFrameworkCore;

[CollectionDefinition(ExamTestConsts.CollectionDefinitionName)]
public class ExamEntityFrameworkCoreCollection : ICollectionFixture<ExamEntityFrameworkCoreFixture>
{

}
