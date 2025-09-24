using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Volo.Abp;

namespace SuperAbp.Exam.BackgroundServices;

public class ExamBackgroundServicesHostedService : IHostedService
{
    private readonly IAbpApplicationWithExternalServiceProvider _application;
    private readonly IServiceProvider _serviceProvider;

    public ExamBackgroundServicesHostedService(
    IAbpApplicationWithExternalServiceProvider application,
    IServiceProvider serviceProvider)
    {
        _application = application;
        _serviceProvider = serviceProvider;
    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _application.Initialize(_serviceProvider);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _application.Shutdown();

        return Task.CompletedTask;
    }
}