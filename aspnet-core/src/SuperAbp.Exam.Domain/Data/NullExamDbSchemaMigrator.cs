using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace SuperAbp.Exam.Data;

/* This is used if database provider does't define
 * IExamDbSchemaMigrator implementation.
 */
public class NullExamDbSchemaMigrator : IExamDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
