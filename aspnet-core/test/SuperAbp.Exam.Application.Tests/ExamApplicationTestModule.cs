using SuperAbp.Exam.Admin;
using Volo.Abp.Modularity;

namespace SuperAbp.Exam;

[DependsOn(
    typeof(ExamApplicationAdminModule),
    typeof(ExamDomainTestModule)
)]
public class ExamApplicationTestModule : AbpModule
{
}