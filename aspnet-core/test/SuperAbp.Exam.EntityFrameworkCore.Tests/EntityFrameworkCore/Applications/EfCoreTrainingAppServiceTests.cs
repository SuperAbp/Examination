using SuperAbp.Exam.Questions;
using SuperAbp.Exam.Trains;
using Xunit;

namespace SuperAbp.Exam.EntityFrameworkCore.Applications;

[Collection(ExamTestConsts.CollectionDefinitionName)]
public class EfCoreTrainingAppServiceTests : TrainingAppServiceTests<ExamEntityFrameworkCoreTestModule>
{
}