using Volo.Abp.Modularity;

namespace SuperAbp.Exam;

[DependsOn(
    typeof(ExamApplicationModule),
    typeof(ExamDomainTestModule)
    )]
public class ExamApplicationTestModule : AbpModule
{

}
