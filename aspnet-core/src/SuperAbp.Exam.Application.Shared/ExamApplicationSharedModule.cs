using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace SuperAbp.Exam;

[DependsOn(
    typeof(ExamApplicationContractsSharedModule),
    typeof(ExamDomainModule),
typeof(AbpDddApplicationModule)
    )]
public class ExamApplicationSharedModule : AbpModule
{
}