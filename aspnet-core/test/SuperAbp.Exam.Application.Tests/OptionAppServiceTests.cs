using System.Collections.Generic;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;
using SuperAbp.Exam.Options;

namespace SuperAbp.Exam;

public abstract class OptionAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly IOptionAppService _optionAppService;

    protected OptionAppServiceTests()
    {
        _optionAppService = GetRequiredService<IOptionAppService>();
    }

    [Fact]
    public void Should_Get_List()
    {
        Dictionary<int, string> result = _optionAppService.GetQuestionTypes();
        result.Count.ShouldBeGreaterThan(0);
    }
}