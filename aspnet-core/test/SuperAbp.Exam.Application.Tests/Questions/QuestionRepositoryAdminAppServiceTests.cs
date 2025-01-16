using Shouldly;
using System.Threading.Tasks;
using SuperAbp.Exam.Admin.QuestionManagement.QuestionRepos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Modularity;
using Xunit;
using SuperAbp.Exam.QuestionManagement.QuestionRepos;
using GetQuestionReposInput = SuperAbp.Exam.Admin.QuestionManagement.QuestionRepos.GetQuestionReposInput;
using QuestionRepoDetailDto = SuperAbp.Exam.Admin.QuestionManagement.QuestionRepos.QuestionRepoDetailDto;
using QuestionRepoListDto = SuperAbp.Exam.Admin.QuestionManagement.QuestionRepos.QuestionRepoListDto;

namespace SuperAbp.Exam.Questions;

public abstract class QuestionRepositoryAdminAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ExamTestData _testData;
    private readonly IQuestionRepoAdminAppService _questionRepoAppService;
    private readonly IQuestionRepoRepository _questionRepoRepository;

    protected QuestionRepositoryAdminAppServiceTests()
    {
        _testData = GetRequiredService<ExamTestData>();
        _questionRepoAppService = GetRequiredService<IQuestionRepoAdminAppService>();
        _questionRepoRepository = GetRequiredService<IQuestionRepoRepository>();
    }

    [Fact]
    public async Task Should_Get_List()
    {
        PagedResultDto<QuestionRepoListDto> result = await _questionRepoAppService.GetListAsync(new GetQuestionReposInput());
        result.Items.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Get_For_Editor()
    {
        GetQuestionRepoForEditorOutput result = await _questionRepoAppService.GetEditorAsync(_testData.QuestionRepository1Id);
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Get()
    {
        QuestionRepoDetailDto result = await _questionRepoAppService.GetAsync(_testData.QuestionRepository1Id);
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Create()
    {
        QuestionRepoCreateDto input = new()
        {
            Title = "New_Title",
            Remark = "New_Remark"
        };
        QuestionRepoListDto dto = await _questionRepoAppService.CreateAsync(input);

        QuestionRepo questionRepo = await _questionRepoRepository.GetAsync(dto.Id);
        questionRepo.ShouldNotBeNull();
        questionRepo.Title.ShouldBe(input.Title);
        questionRepo.Remark.ShouldBe(input.Remark);
    }

    [Fact]
    public async Task Should_Create_Throw_Exist_Title()
    {
        QuestionRepoCreateDto dto = new()
        {
            Title = _testData.QuestionRepository1Title,
            Remark = "New_Remark"
        };
        await Should.ThrowAsync<QuestionRepositoryTitleAlreadyExistException>(
            async () => await _questionRepoAppService.CreateAsync(dto));
    }

    [Fact]
    public async Task Should_Update()
    {
        QuestionRepoUpdateDto input = new()
        {
            Title = "Update_Title",
            Remark = "Update_Remark"
        };
        await _questionRepoAppService.UpdateAsync(_testData.QuestionRepository1Id, input);
        QuestionRepo questionRepo = await _questionRepoRepository.GetAsync(_testData.QuestionRepository1Id);
        questionRepo.ShouldNotBeNull();
        questionRepo.Title.ShouldBe(input.Title);
        questionRepo.Remark.ShouldBe(input.Remark);
    }

    [Fact]
    public async Task Should_Update_Throw_Exist_Title()
    {
        QuestionRepoUpdateDto dto = new()
        {
            Title = _testData.QuestionRepository2Title,
            Remark = "Update_Remark"
        };
        await Should.ThrowAsync<QuestionRepositoryTitleAlreadyExistException>(
            async () => await _questionRepoAppService.UpdateAsync(_testData.QuestionRepository1Id, dto));
    }

    [Fact]
    public async Task Should_Delete()
    {
        await _questionRepoAppService.DeleteAsync(_testData.QuestionRepository1Id);
        await Should.ThrowAsync<EntityNotFoundException>(
            async () =>
                await _questionRepoAppService.GetEditorAsync(_testData.QuestionRepository1Id));
    }
}