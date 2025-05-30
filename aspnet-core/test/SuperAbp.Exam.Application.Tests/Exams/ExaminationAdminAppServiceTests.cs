using System;
using Shouldly;
using SuperAbp.Exam.Admin.ExamManagement.Exams;
using System.Threading.Tasks;
using SuperAbp.Exam.ExamManagement.Exams;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Modularity;
using Xunit;
using ExamListDto = SuperAbp.Exam.Admin.ExamManagement.Exams.ExamListDto;
using GetExamsInput = SuperAbp.Exam.Admin.ExamManagement.Exams.GetExamsInput;

namespace SuperAbp.Exam.Exams;

public abstract class ExaminationAdminAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ExamTestData _testData;
    private readonly IExaminationAdminAppService _examinationAppService;
    private IExamRepository _examRepository;

    protected ExaminationAdminAppServiceTests()
    {
        _testData = GetRequiredService<ExamTestData>();
        _examinationAppService = GetRequiredService<IExaminationAdminAppService>();
        _examRepository = GetRequiredService<IExamRepository>();
    }

    [Fact]
    public async Task Should_Get_List()
    {
        PagedResultDto<ExamListDto> result = await _examinationAppService.GetListAsync(new GetExamsInput());
        result.Items.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Get_For_Editor()
    {
        GetExamForEditorOutput result = await _examinationAppService.GetEditorAsync(_testData.Examination11Id);
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Create()
    {
        ExamCreateDto input = new()
        {
            Name = "New_Name",
            Description = "New_Description",
            PaperId = _testData.Paper1Id,
            Score = 100,
            PassingScore = 60,
            TotalTime = 100,
            StartTime = DateTime.Now.AddDays(1),
            EndTime = DateTime.Now.AddDays(2)
        };
        ExamListDto dto = await _examinationAppService.CreateAsync(input);
        Examination examination = await _examRepository.GetAsync(dto.Id);
        examination.ShouldNotBeNull();
        examination.Name.ShouldBe(input.Name);
        examination.Description.ShouldBe(input.Description);
        examination.PaperId.ShouldBe(input.PaperId);
        examination.Score.ShouldBe(input.Score);
        examination.PassingScore.ShouldBe(input.PassingScore);
        examination.StartTime.ShouldBe(input.StartTime);
        examination.EndTime.ShouldBe(input.EndTime);
        examination.Status.Value.ShouldBe(ExaminationStatus.Draft.Value);
    }

    [Fact]
    public async Task Should_Update()
    {
        ExamUpdateDto input = new()
        {
            Name = "Update_Name",
            Description = "Update_Description",
            PaperId = _testData.Paper2Id,
            Score = int.MaxValue,
            PassingScore = int.MaxValue,
            TotalTime = int.MaxValue,
            StartTime = DateTime.MaxValue.AddDays(-1),
            EndTime = DateTime.MaxValue
        };
        await _examinationAppService.UpdateAsync(_testData.Examination11Id, input);
        Examination examination = await _examRepository.GetAsync(_testData.Examination11Id);
        examination.ShouldNotBeNull();
        examination.Name.ShouldBe(input.Name);
        examination.Description.ShouldBe(input.Description);
        examination.PaperId.ShouldBe(input.PaperId);
        examination.Score.ShouldBe(input.Score);
        examination.PassingScore.ShouldBe(input.PassingScore);
        examination.StartTime.ShouldBe(input.StartTime);
        examination.EndTime.ShouldBe(input.EndTime);
        examination.Status.Value.ShouldBe(ExaminationStatus.Draft.Value);
    }

    [Fact]
    public async Task Should_Publish()
    {
        await _examinationAppService.PublishAsync(_testData.Examination11Id);
    }

    [Fact]
    public async Task Should_Publish_Throw_InvalidExamStatusException()
    {
        await Should.ThrowAsync<InvalidExamStatusException>(async () =>
            await _examinationAppService.PublishAsync(_testData.Examination12Id));
    }

    [Fact]
    public async Task Should_Cancel()
    {
        await _examinationAppService.CancelAsync(_testData.Examination12Id);
    }

    [Fact]
    public async Task Should_Cancel_Throw_InvalidExamStatusException()
    {
        await Should.ThrowAsync<InvalidExamStatusException>(async () =>
            await _examinationAppService.CancelAsync(_testData.Examination15Id));
    }

    [Fact]
    public async Task Should_Delete()
    {
        await _examinationAppService.DeleteAsync(_testData.Examination11Id);
        await Should.ThrowAsync<EntityNotFoundException>(
            async () =>
                await _examinationAppService.GetEditorAsync(_testData.Examination11Id));
    }
}