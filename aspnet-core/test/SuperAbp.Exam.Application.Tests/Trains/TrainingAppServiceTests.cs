using SuperAbp.Exam.TrainingManagement;
using System.Threading.Tasks;
using System;
using Shouldly;
using Xunit;
using Volo.Abp.Modularity;
using SuperAbp.Exam.ExamManagement.UserExams;

namespace SuperAbp.Exam.Trains;

public abstract class TrainingAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ExamTestData _testData;
    private readonly ITrainingAppService _trainingAppService;
    private readonly ITrainingRepository _trainingRepository;

    protected TrainingAppServiceTests()
    {
        _testData = GetRequiredService<ExamTestData>();
        _trainingAppService = GetRequiredService<ITrainingAppService>();
        _trainingRepository = GetRequiredService<ITrainingRepository>();
    }

    [Fact]
    public async Task Should_Get_List()
    {
        var result = await _trainingAppService.GetListAsync(new GetTrainsInput { QuestionRepositoryId = _testData.QuestionRepository1Id });
        result.Items.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Create()
    {
        var input = new TrainingCreateDto
        {
            TrainingSource = TrainingSource.QuestionRepository,
            QuestionRepositoryId = _testData.QuestionRepository1Id,
            QuestionId = _testData.Question12Id,
            Right = true
        };

        TrainingListDto dto = await _trainingAppService.CreateAsync(input);
        Training result = await _trainingRepository.GetAsync(dto.Id);
        result.ShouldNotBeNull();
        result.QuestionId.ShouldBe(input.QuestionId);
        result.Right.ShouldBe(input.Right);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Should_Set_Is_Right(bool right)
    {
        await _trainingAppService.SetIsRightAsync(_testData.Training1Id, right);
        Training result = await _trainingRepository.GetAsync(_testData.Training1Id);
        result.Right.ShouldBe(right);
    }
}