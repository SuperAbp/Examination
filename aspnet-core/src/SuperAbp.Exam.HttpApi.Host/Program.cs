using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace SuperAbp.Exam;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
#if DEBUG
                .WriteTo.Logger(lg => lg
                    // 过滤级别
                    .Filter.ByIncludingOnly(p => p.Level.Equals(LogEventLevel.Debug))
                    // 按日期/大小分割文件
                    .WriteTo.Async(a => a.File("Logs/debug.log", LogEventLevel.Debug
                        , rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)))
                .WriteTo.Console()
#endif
            .WriteTo.Logger(lg => lg
                    .Filter.ByIncludingOnly(p => p.Level.Equals(LogEventLevel.Information))
                    .WriteTo.Async(a => a.File("Logs/info.log", LogEventLevel.Information
                        , rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)))
                    .WriteTo.Console()
                 .WriteTo.Logger(lg => lg
                     .Filter.ByIncludingOnly(p => p.Level.Equals(LogEventLevel.Error))
                     .WriteTo.Async(a => a.File("Logs/error.log", LogEventLevel.Error
                         , rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)))
                .CreateLogger();

        try
        {
            Log.Information("Starting SuperAbp.Exam.HttpApi.Host.");
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.AddAppSettingsSecretsJson()
                .UseAutofac()
                .UseSerilog();
            await builder.AddApplicationAsync<ExamHttpApiHostModule>();
            var app = builder.Build();
            await app.InitializeApplicationAsync();
            await app.RunAsync();
            return 0;
        }
        catch (Exception ex)
        {
            if (ex is HostAbortedException)
            {
                throw;
            }

            Log.Fatal(ex, "Host terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
