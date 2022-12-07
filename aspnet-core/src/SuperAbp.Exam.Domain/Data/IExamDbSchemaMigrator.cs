using System.Threading.Tasks;

namespace SuperAbp.Exam.Data;

public interface IExamDbSchemaMigrator
{
    Task MigrateAsync();
}
