using SuperAbp.Exam.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace SuperAbp.Exam;

[DependsOn(
    typeof(ExamEntityFrameworkCoreTestModule)
    )]
public class ExamDomainTestModule : AbpModule
{

}
