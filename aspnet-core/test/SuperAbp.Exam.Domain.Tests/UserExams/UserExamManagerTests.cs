using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shouldly;
using SuperAbp.Exam.ExamManagement.Exams;
using SuperAbp.Exam.ExamManagement.UserExamQuestions;
using SuperAbp.Exam.ExamManagement.UserExams;
using SuperAbp.Exam.QuestionManagement.Questions;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;
using Xunit;
using static SuperAbp.Exam.ExamDomainErrorCodes;

namespace SuperAbp.Exam.UserExams;

public abstract class UserExamManagerTests<TStartupModule> : ExamDomainTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ExamTestData _testData;
    private readonly UserExamManager _userExamManager;
    private IUserExamRepository _userExamRepository;

    public UserExamManagerTests()
    {
        _testData = GetRequiredService<ExamTestData>();
        _userExamManager = GetRequiredService<UserExamManager>();
        _userExamRepository = GetRequiredService<IUserExamRepository>();
    }

    [Fact]
    public async Task Should_Create_Questions()
    {
        await WithUnitOfWorkAsync(async () =>
        {
            await _userExamManager.CreateQuestionsAsync(_testData.UserExam31Id);
        });

        UserExam userExam = await _userExamRepository.GetAsync(_testData.UserExam31Id);
        userExam.ShouldNotBeNull();
        userExam.Questions.Count.ShouldBeGreaterThan(0);
    }
}