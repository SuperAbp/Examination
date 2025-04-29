using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using SuperAbp.Exam.Admin.PaperManagement.Papers;
using SuperAbp.Exam.PaperManagement.PaperQuestionRules;
using SuperAbp.Exam.PaperManagement.Papers;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Xunit;

namespace SuperAbp.Exam.Papers;

public abstract class PaperAdminAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ExamTestData _testData;
    private readonly IPaperAdminAppService _paperAdminAppService;
    private readonly IPaperRepository _paperRepository;
    private readonly IPaperQuestionRuleRepository _paperRepoRepository;

    protected PaperAdminAppServiceTests()
    {
        _testData = GetRequiredService<ExamTestData>();
        _paperAdminAppService = GetRequiredService<IPaperAdminAppService>();
        _paperRepository = GetRequiredService<IPaperRepository>();
        _paperRepoRepository = GetRequiredService<IPaperQuestionRuleRepository>();
    }

    [Fact]
    public async Task Should_Get_List()
    {
        PagedResultDto<PaperListDto> result = await _paperAdminAppService.GetListAsync(new GetPapersInput());
        result.Items.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Get_For_Editor()
    {
        GetPaperForEditorOutput result = await _paperAdminAppService.GetEditorAsync(_testData.Paper1Id);
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Create()
    {
        PaperCreateOrUpdatePaperQuestionRuleDto[] repositories =
        [
            new PaperCreateOrUpdatePaperQuestionRuleDto()
            {
                QuestionBankId = _testData.QuestionBank1Id,
                SingleCount = 1,
                SingleScore = 1,
                MultiCount = 1,
                MultiScore = 1,
                JudgeCount = 1,
                JudgeScore = 1,
                BlankCount = 1,
                BlankScore = 1
            }
        ];
        PaperCreateDto input = new()
        {
            Name = "New_Name",
            Description = "New_Description",
            Score = repositories.Sum(r => r.SingleScore + r.MultiScore + r.JudgeScore + r.BlankScore) ?? 0,
            PaperQuestionRules = repositories
        };
        PaperListDto dto = await _paperAdminAppService.CreateAsync(input);
        Paper paper = await _paperRepository.GetAsync(dto.Id);
        paper.ShouldNotBeNull();
        paper.Name.ShouldBe(input.Name);
        paper.Description.ShouldBe(input.Description);
        paper.Score.ShouldBe(input.Score);
        List<PaperQuestionRule> paperRepos = await _paperRepoRepository.GetListAsync(paperId: paper.Id);
        paperRepos.Count.ShouldBe(input.PaperQuestionRules.Length);
    }

    [Fact]
    public async Task Should_Create_Throw_Exists_Content()
    {
        PaperCreateOrUpdatePaperQuestionRuleDto[] repositories =
        [
            new PaperCreateOrUpdatePaperQuestionRuleDto()
            {
                QuestionBankId = _testData.QuestionBank1Id,
                SingleCount = 1,
                SingleScore = 1,
                MultiCount = 1,
                MultiScore = 1,
                JudgeCount = 1,
                JudgeScore = 1,
                BlankCount = 1,
                BlankScore = 1
            }
        ];
        PaperCreateDto input = new()
        {
            Name = _testData.Paper1Name,
            Description = "New_Description",
            Score = repositories.Sum(r => r.SingleScore + r.MultiScore + r.JudgeScore + r.BlankScore) ?? 0,
            PaperQuestionRules = repositories
        };
        await Should.ThrowAsync<PaperNameAlreadyExistException>(
            async () => await _paperAdminAppService.CreateAsync(input));
    }

    [Fact]
    public async Task Should_Create_Throw_Required_Repository()
    {
        PaperCreateDto input = new()
        {
            Name = _testData.Paper1Name,
            Description = "New_Description",
            Score = 0
        };
        await Should.ThrowAsync<AbpValidationException>(
            async () => await _paperAdminAppService.CreateAsync(input));
    }

    [Fact]
    public async Task Should_Create_Throw_Invalid_Score()
    {
        PaperCreateDto input = new()
        {
            Name = _testData.Paper1Name,
            Description = "New_Description",
            Score = 0,
            PaperQuestionRules = [
                new PaperCreateOrUpdatePaperQuestionRuleDto()
                {
                    QuestionBankId = _testData.QuestionBank1Id,
                    SingleCount = 1,
                    SingleScore = 1,
                    MultiCount = 1,
                    MultiScore = 1,
                    JudgeCount = 1,
                    JudgeScore = 1,
                    BlankCount = 1,
                    BlankScore = 1
                }
            ]
        };
        await Should.ThrowAsync<AbpValidationException>(
            async () => await _paperAdminAppService.CreateAsync(input));
    }

    [Fact]
    public async Task Should_Update()
    {
        PaperCreateOrUpdatePaperQuestionRuleDto[] repositories =
        [
            new PaperCreateOrUpdatePaperQuestionRuleDto()
            {
                QuestionBankId = _testData.QuestionBank1Id,
                SingleCount = 1,
                SingleScore = 1,
                MultiCount = 1,
                MultiScore = 1,
                JudgeCount = 1,
                JudgeScore = 1,
                BlankCount = 1,
                BlankScore = 1
            },
            new PaperCreateOrUpdatePaperQuestionRuleDto()
            {
                QuestionBankId = _testData.QuestionBank2Id,
                SingleCount = 1,
                SingleScore = 1,
                MultiCount = 1,
                MultiScore = 1,
                JudgeCount = 1,
                JudgeScore = 1,
                BlankCount = 1,
                BlankScore = 1
            }
        ];
        PaperUpdateDto input = new()
        {
            Name = "Update_Name",
            Description = "Update_Description",
            Score = repositories.Sum(r => r.SingleScore + r.MultiScore + r.JudgeScore + r.BlankScore) ?? 0,
            PaperQuestionRules = repositories
        };
        await _paperAdminAppService.UpdateAsync(_testData.Paper1Id, input);
        Paper paper = await _paperRepository.GetAsync(_testData.Paper1Id);
        paper.ShouldNotBeNull();
        paper.Name.ShouldBe(input.Name);
        paper.Description.ShouldBe(input.Description);
        paper.Score.ShouldBe(input.Score);
        List<PaperQuestionRule> paperRepos = await _paperRepoRepository.GetListAsync(paperId: paper.Id);
    }

    [Fact]
    public async Task Should_Update_Throw_Required_Repository()
    {
        PaperUpdateDto input = new()
        {
            Name = _testData.Paper1Name,
            Description = "New_Description",
            Score = 0
        };
        await Should.ThrowAsync<AbpValidationException>(
            async () => await _paperAdminAppService.UpdateAsync(_testData.Paper1Id, input));
    }

    [Fact]
    public async Task Should_Update_Throw_Invalid_Score()
    {
        PaperUpdateDto input = new()
        {
            Name = _testData.Paper1Name,
            Description = "New_Description",
            Score = 0,
            PaperQuestionRules = [
                new PaperCreateOrUpdatePaperQuestionRuleDto()
                {
                    QuestionBankId = _testData.QuestionBank1Id,
                    SingleCount = 1,
                    SingleScore = 1,
                    MultiCount = 1,
                    MultiScore = 1,
                    JudgeCount = 1,
                    JudgeScore = 1,
                    BlankCount = 1,
                    BlankScore = 1
                }
            ]
        };
        await Should.ThrowAsync<AbpValidationException>(
            async () => await _paperAdminAppService.UpdateAsync(_testData.Paper1Id, input));
    }

    [Fact]
    public async Task Should_Update_Throw_Exists_Content()
    {
        PaperCreateOrUpdatePaperQuestionRuleDto[] repositories =
        [
            new PaperCreateOrUpdatePaperQuestionRuleDto()
            {
                QuestionBankId = _testData.QuestionBank1Id,
                SingleCount = 1,
                SingleScore = 1,
                MultiCount = 1,
                MultiScore = 1,
                JudgeCount = 1,
                JudgeScore = 1,
                BlankCount = 1,
                BlankScore = 1
            },
            new PaperCreateOrUpdatePaperQuestionRuleDto()
            {
                QuestionBankId = _testData.QuestionBank2Id,
                SingleCount = 1,
                SingleScore = 1,
                MultiCount = 1,
                MultiScore = 1,
                JudgeCount = 1,
                JudgeScore = 1,
                BlankCount = 1,
                BlankScore = 1
            }
        ];
        PaperUpdateDto input = new()
        {
            Name = _testData.Paper2Name,
            Description = "Update_Description",
            Score = repositories.Sum(r => r.SingleScore + r.MultiScore + r.JudgeScore + r.BlankScore) ?? 0,
            PaperQuestionRules = repositories
        };
        await Should.ThrowAsync<PaperNameAlreadyExistException>(
            async () => await _paperAdminAppService.UpdateAsync(_testData.Paper1Id, input));
    }

    [Fact]
    public async Task Should_Delete()
    {
        await _paperAdminAppService.DeleteAsync(_testData.Paper1Id);
        await Should.ThrowAsync<EntityNotFoundException>(
            async () =>
                await _paperAdminAppService.GetEditorAsync(_testData.Paper1Id));
    }
}