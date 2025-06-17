using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using SuperAbp.Exam.ExamManagement.Exams;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Modularity;
using Xunit;

namespace SuperAbp.Exam.Exams;

public abstract class ExaminationAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ExamTestData _testData;
    private readonly IExaminationAppService _examinationAppService;

    protected ExaminationAppServiceTests()
    {
        _testData = GetRequiredService<ExamTestData>();
        _examinationAppService = GetRequiredService<IExaminationAppService>();
    }

    [Fact]
    public async Task Should_Get_List()
    {
        PagedResultDto<ExamListDto> result = await _examinationAppService.GetListAsync(new GetExamsInput());
        result.Items.Count.ShouldBeGreaterThan(0);
        result.Items.Where(c => c.Status != ExaminationStatus.Published).ShouldBeEmpty();
    }

    [Fact]
    public async Task Should_Get()
    {
        ExamDetailDto result = await _examinationAppService.GetAsync(_testData.Examination11Id);
        result.ShouldNotBeNull();
    }
}