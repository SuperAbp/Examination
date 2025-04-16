using Shouldly;
using System.Threading.Tasks;
using System;
using SuperAbp.Exam.Admin.QuestionManagement.QuestionCategories;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Modularity;
using Xunit;
using SuperAbp.Exam.QuestionManagement.QuestionCategories;
using Volo.Abp.Domain.Entities;

namespace SuperAbp.Exam.Questions;

public abstract class QuestionCategoryAdminAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ExamTestData _testData;
    private readonly IQuestionCategoryAdminAppService _questionCategoryAppService;
    private readonly IQuestionCategoryRepository _questionCategoryRepository;

    protected QuestionCategoryAdminAppServiceTests()
    {
        _testData = GetRequiredService<ExamTestData>();
        _questionCategoryAppService = GetRequiredService<IQuestionCategoryAdminAppService>();
        _questionCategoryRepository = GetRequiredService<IQuestionCategoryRepository>();
    }

    [Fact]
    public async Task Should_Get_List()
    {
        PagedResultDto<QuestionCategoryListDto> result = await _questionCategoryAppService.GetListAsync(new GetQuestionCategoriesInput());
        result.Items.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Get()
    {
        GetQuestionCategoryForEditorOutput result = await _questionCategoryAppService.GetEditorAsync(_testData.QuestionCategory1Id);
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task Should_Create()
    {
        QuestionCategoryCreateDto input = new()
        {
            Name = "New Category"
        };
        Guid id = await _questionCategoryAppService.CreateAsync(input);
        QuestionCategory category = await _questionCategoryRepository.GetAsync(id);
        category.ShouldNotBeNull();
        category.Name.ShouldBe(input.Name);
    }

    [Fact]
    public async Task Should_Create_Child()
    {
        QuestionCategoryCreateDto input = new()
        {
            Name = "New Category",
            ParentId = _testData.QuestionCategory1Id
        };
        Guid id = await _questionCategoryAppService.CreateAsync(input);
        QuestionCategory category = await _questionCategoryRepository.GetAsync(id);
        category.ShouldNotBeNull();
        category.Name.ShouldBe(input.Name);
    }

    [Fact]
    public async Task Should_Update()
    {
        QuestionCategoryUpdateDto input = new()
        {
            Name = "New Category"
        };
        await _questionCategoryAppService.UpdateAsync(_testData.QuestionCategory1Id, input);
        QuestionCategory questionCategory = await _questionCategoryRepository.GetAsync(_testData.QuestionCategory1Id);
        questionCategory.ShouldNotBeNull();
        questionCategory.Name.ShouldBe(input.Name);
    }

    [Fact]
    public async Task Should_Update_Child()
    {
        QuestionCategoryUpdateDto input = new()
        {
            Name = "New Category",
            ParentId = _testData.QuestionCategory1Id
        };
        await _questionCategoryAppService.UpdateAsync(_testData.QuestionCategory2Id, input);
        QuestionCategory questionCategory = await _questionCategoryRepository.GetAsync(_testData.QuestionCategory2Id);
        questionCategory.ShouldNotBeNull();
        questionCategory.Name.ShouldBe(input.Name);
    }

    [Fact]
    public async Task Should_Delete()
    {
        await _questionCategoryAppService.DeleteAsync(_testData.Question11Id);
        await Should.ThrowAsync<EntityNotFoundException>(
            async () =>
                await _questionCategoryAppService.GetEditorAsync(_testData.Question11Id));
    }
}