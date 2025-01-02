using System;
using System.Runtime.InteropServices;
using Shouldly;
using SuperAbp.Exam.Admin.ExamManagement.Exams;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Modularity;
using Xunit;

namespace SuperAbp.Exam.Questions;

public class ExaminationAdminAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ExamTestData _testData;
    private readonly IExaminationAdminAppService _examinationAppService;

    protected ExaminationAdminAppServiceTests()
    {
        _testData = GetRequiredService<ExamTestData>();
        _examinationAppService = GetRequiredService<IExaminationAdminAppService>();
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
        ExamCreateDto createDto = new()
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
        ExamListDto dto = await _examinationAppService.CreateAsync(createDto);
        GetExamForEditorOutput examination = await _examinationAppService.GetEditorAsync(dto.Id);
        examination.ShouldNotBeNull();
        examination.Name.ShouldBe(createDto.Name);
        examination.Description.ShouldBe(createDto.Description);
        examination.PaperId.ShouldBe(createDto.PaperId);
        examination.Score.ShouldBe(createDto.Score);
        examination.PassingScore.ShouldBe(createDto.PassingScore);
        examination.StartTime.ShouldBe(createDto.StartTime);
        examination.EndTime.ShouldBe(createDto.EndTime);
    }

    [Fact]
    public async Task Should_Update()
    {
        ExamUpdateDto updateDto = new()
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
        await _examinationAppService.UpdateAsync(_testData.Examination11Id, updateDto);
        GetExamForEditorOutput examination = await _examinationAppService.GetEditorAsync(_testData.Examination11Id);
        examination.ShouldNotBeNull();
        examination.Name.ShouldBe(updateDto.Name);
        examination.Description.ShouldBe(updateDto.Description);
        examination.PaperId.ShouldBe(updateDto.PaperId);
        examination.Score.ShouldBe(updateDto.Score);
        examination.PassingScore.ShouldBe(updateDto.PassingScore);
        examination.StartTime.ShouldBe(updateDto.StartTime);
        examination.EndTime.ShouldBe(updateDto.EndTime);
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