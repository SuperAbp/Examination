using SuperAbp.Exam.UserExams;
using Xunit;

namespace SuperAbp.Exam.EntityFrameworkCore.Domains;

[Collection(ExamTestConsts.CollectionDefinitionName)]
public class EfCoreUserExamManagerTests : UserExamManagerTests<ExamEntityFrameworkCoreTestModule>
{
}