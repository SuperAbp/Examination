using Microsoft.Extensions.Options;
using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Timing;

namespace SuperAbp.Exam;

public class FakeClock : IClock, ITransientDependency
{
    public DateTime Normalize(DateTime dateTime) => dateTime;

    public DateTime FakeNow { get; set; } = DateTime.Now;
    public DateTime Now => FakeNow;
    public DateTimeKind Kind => DateTimeKind.Utc;
    public bool SupportsMultipleTimezone => DateTimeKind.Utc == DateTimeKind.Utc;
}