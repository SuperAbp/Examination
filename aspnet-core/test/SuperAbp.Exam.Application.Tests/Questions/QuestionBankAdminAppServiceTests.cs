using Shouldly;
using System.Threading.Tasks;
using SuperAbp.Exam.Admin.QuestionManagement.QuestionBanks;
using SuperAbp.Exam.QuestionManagement.QuestionBanks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Modularity;
using Xunit;
using GetQuestionBanksInput = SuperAbp.Exam.Admin.QuestionManagement.QuestionBanks.GetQuestionBanksInput;
using QuestionBankDetailDto = SuperAbp.Exam.Admin.QuestionManagement.QuestionBanks.QuestionBankDetailDto;
using QuestionBankListDto = SuperAbp.Exam.Admin.QuestionManagement.QuestionBanks.QuestionBankListDto;

namespace SuperAbp.Exam.Questions;

public abstract class QuestionBankAdminAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ExamTestData _testData;
    private readonly IQuestionBankAdminAppService _questionBankAppService;
    private readonly IQuestionBankRepository _questionBankRepository;

    protected QuestionBankAdminAppServiceTests()
    {
        _testData = GetRequiredService<ExamTestData>();
        _questionBankAppService = GetRequiredService<IQuestionBankAdminAppService>();
        _questionBankRepository = GetRequiredService<IQuestionBankRepository>();
    }

    [Fact]
    public async Task Should_Get_List()
    {
        PagedResultDto<QuestionBankListDto> result = await _questionBankAppService.GetListAsync(new GetQuestionBanksInput());
        result.Items.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Get_For_Editor()
    {
        GetQuestionBankForEditorOutput result = await _questionBankAppService.GetEditorAsync(_testData.QuestionBank1Id);
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Get()
    {
        QuestionBankDetailDto result = await _questionBankAppService.GetAsync(_testData.QuestionBank1Id);
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Create()
    {
        QuestionBankCreateDto input = new()
        {
            Title = "New_Title",
            Remark = "New_Remark"
        };
        QuestionBankListDto dto = await _questionBankAppService.CreateAsync(input);

        QuestionBank questionRepo = await _questionBankRepository.GetAsync(dto.Id);
        questionRepo.ShouldNotBeNull();
        questionRepo.Title.ShouldBe(input.Title);
        questionRepo.Remark.ShouldBe(input.Remark);
    }

    [Fact]
    public async Task Should_Create_Throw_Exist_Title()
    {
        QuestionBankCreateDto dto = new()
        {
            Title = _testData.QuestionBank1Title,
            Remark = "New_Remark"
        };
        await Should.ThrowAsync<QuestionBankTitleAlreadyExistException>(
            async () => await _questionBankAppService.CreateAsync(dto));
    }

    [Fact]
    public async Task Should_Update()
    {
        QuestionBankUpdateDto input = new()
        {
            Title = "Update_Title",
            Remark = "Update_Remark"
        };
        await _questionBankAppService.UpdateAsync(_testData.QuestionBank1Id, input);
        QuestionBank questionRepo = await _questionBankRepository.GetAsync(_testData.QuestionBank1Id);
        questionRepo.ShouldNotBeNull();
        questionRepo.Title.ShouldBe(input.Title);
        questionRepo.Remark.ShouldBe(input.Remark);
    }

    [Fact]
    public async Task Should_Update_Throw_Exist_Title()
    {
        QuestionBankUpdateDto dto = new()
        {
            Title = _testData.QuestionBank2Title,
            Remark = "Update_Remark"
        };
        await Should.ThrowAsync<QuestionBankTitleAlreadyExistException>(
            async () => await _questionBankAppService.UpdateAsync(_testData.QuestionBank1Id, dto));
    }

    [Fact]
    public async Task Should_Delete()
    {
        await _questionBankAppService.DeleteAsync(_testData.QuestionBank1Id);
        await Should.ThrowAsync<EntityNotFoundException>(
            async () =>
                await _questionBankAppService.GetEditorAsync(_testData.QuestionBank1Id));
    }
}