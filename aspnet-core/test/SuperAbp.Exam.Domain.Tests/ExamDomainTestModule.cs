using Volo.Abp.Modularity;

namespace SuperAbp.Exam;

[DependsOn(
    typeof(ExamDomainModule),
    typeof(ExamTestBaseModule)
)]
public class ExamDomainTestModule : AbpModule
{

}
