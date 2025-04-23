using Shouldly;
using System.Threading.Tasks;
using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Modularity;
using Xunit;
using Volo.Abp.Domain.Entities;
using SuperAbp.Exam.Admin.KnowledgePoints;
using SuperAbp.Exam.KnowledgePoints;

namespace SuperAbp.Exam.Questions;

public abstract class KnowledgePointAdminAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ExamTestData _testData;
    private readonly IKnowledgePointAdminAppService _knowledgePointAppService;
    private readonly IKnowledgePointRepository _knowledgePointRepository;

    protected KnowledgePointAdminAppServiceTests()
    {
        _testData = GetRequiredService<ExamTestData>();
        _knowledgePointAppService = GetRequiredService<IKnowledgePointAdminAppService>();
        _knowledgePointRepository = GetRequiredService<IKnowledgePointRepository>();
    }

    [Fact]
    public async Task Should_Get_All()
    {
        ListResultDto<KnowledgePointListDto> result = await _knowledgePointAppService.GetAllAsync(new GetKnowledgePointsInput());
        result.Items.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Get()
    {
        GetKnowledgePointForEditorOutput result = await _knowledgePointAppService.GetEditorAsync(_testData.KnowledgePoint1Id);
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Create()
    {
        KnowledgePointCreateDto input = new()
        {
            Name = "New Name"
        };
        Guid id = await _knowledgePointAppService.CreateAsync(input);
        KnowledgePoint knowledgePoint = await _knowledgePointRepository.GetAsync(id);
        knowledgePoint.ShouldNotBeNull();
        knowledgePoint.Name.ShouldBe(input.Name);
    }

    [Fact]
    public async Task Should_Create_Child()
    {
        KnowledgePointCreateDto input = new()
        {
            Name = "New Name",
            ParentId = _testData.KnowledgePoint1Id
        };
        Guid id = await _knowledgePointAppService.CreateAsync(input);
        KnowledgePoint knowledgePoint = await _knowledgePointRepository.GetAsync(id);
        knowledgePoint.ShouldNotBeNull();
        knowledgePoint.Name.ShouldBe(input.Name);
    }

    [Fact]
    public async Task Should_Update()
    {
        KnowledgePointUpdateDto input = new()
        {
            Name = "New Category"
        };
        await _knowledgePointAppService.UpdateAsync(_testData.KnowledgePoint1Id, input);
        KnowledgePoint knowledgePoint = await _knowledgePointRepository.GetAsync(_testData.KnowledgePoint1Id);
        knowledgePoint.ShouldNotBeNull();
        knowledgePoint.Name.ShouldBe(input.Name);
    }

    [Fact]
    public async Task Should_Update_Child()
    {
        KnowledgePointUpdateDto input = new()
        {
            Name = "New Category",
            ParentId = _testData.KnowledgePoint1Id
        };
        await _knowledgePointAppService.UpdateAsync(_testData.KnowledgePoint2Id, input);
        KnowledgePoint knowledgePoint = await _knowledgePointRepository.GetAsync(_testData.KnowledgePoint2Id);
        knowledgePoint.ShouldNotBeNull();
        knowledgePoint.Name.ShouldBe(input.Name);
    }

    [Fact]
    public async Task Should_Delete()
    {
        await _knowledgePointAppService.DeleteAsync(_testData.Question11Id);
        await Should.ThrowAsync<EntityNotFoundException>(
            async () =>
                await _knowledgePointAppService.GetEditorAsync(_testData.Question11Id));
    }
}