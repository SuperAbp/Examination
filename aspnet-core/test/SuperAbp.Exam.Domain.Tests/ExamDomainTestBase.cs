using Volo.Abp.Modularity;

namespace SuperAbp.Exam;

/* Inherit from this class for your domain layer tests. */
public abstract class ExamDomainTestBase<TStartupModule> : ExamTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
