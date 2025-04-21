using System.Threading.Tasks;
using Shouldly;
using SuperAbp.Exam.Admin.PaperManagement.PaperQuestionRules;
using SuperAbp.Exam.PaperManagement.PaperQuestionRules;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Modularity;
using Xunit;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Validation;

namespace SuperAbp.Exam.Papers;

public abstract class PaperQuestionRuleAdminAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ExamTestData _testData;
    private readonly IPaperQuestionRuleAdminAppService _paperQuestionRuleAdminAppService;

    protected PaperQuestionRuleAdminAppServiceTests()
    {
        _testData = GetRequiredService<ExamTestData>();
        _paperQuestionRuleAdminAppService = GetRequiredService<IPaperQuestionRuleAdminAppService>();
    }

    [Fact]
    public async Task Should_Get_List()
    {
        PagedResultDto<PaperQuestionRuleListDto> result = await _paperQuestionRuleAdminAppService.GetListAsync(new GetPaperQuestionRulesInput() { PaperId = _testData.Paper1Id });
        result.Items.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Get_List_Throw_Not_Validation()
    {
        await Should.ThrowAsync<AbpValidationException>(
            async () => await _paperQuestionRuleAdminAppService.GetListAsync(new GetPaperQuestionRulesInput()));
    }

    [Fact]
    public async Task Should_Get_For_Editor()
    {
        GetPaperQuestionRuleForEditorOutput result = await _paperQuestionRuleAdminAppService.GetEditorAsync(_testData.PaperQuestionRule1Id);
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Delete()
    {
        await _paperQuestionRuleAdminAppService.DeleteAsync(_testData.PaperQuestionRule1Id);
        await Should.ThrowAsync<EntityNotFoundException>(
            async () =>
                await _paperQuestionRuleAdminAppService.GetEditorAsync(_testData.PaperQuestionRule1Id));
    }
}