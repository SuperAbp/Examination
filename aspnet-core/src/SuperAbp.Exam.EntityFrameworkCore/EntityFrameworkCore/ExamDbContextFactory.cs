using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SuperAbp.Exam.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */

public class ExamDbContextFactory : IDesignTimeDbContextFactory<ExamDbContext>
{
    public ExamDbContext CreateDbContext(string[] args)
    {
        ExamEfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        string connectionString = configuration.GetConnectionString("Default") ?? String.Empty;
        var builder = new DbContextOptionsBuilder<ExamDbContext>()
            .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        return new ExamDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../SuperAbp.Exam.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}