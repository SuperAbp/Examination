using Volo.Abp.Modularity;

namespace SuperAbp.Exam;

public abstract class ExamApplicationTestBase<TStartupModule> : ExamTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
