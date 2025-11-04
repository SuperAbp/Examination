using SuperAbp.Exam.ExamManagement.Exams;
using SuperAbp.Exam.ExamManagement.UserExamQuestions;
using SuperAbp.Exam.PaperManagement.PaperQuestionRules;
using SuperAbp.Exam.PaperManagement.Papers;
using SuperAbp.Exam.PaperManagement.PaperSections;
using SuperAbp.Exam.QuestionManagement.Questions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Local;

namespace SuperAbp.Exam.ExamManagement.UserExams;

public class UserExamManager(
    ExamManager examManager,
    IExamRepository examRepository,
    IQuestionRepository questionRepository,
    IPaperRepository paperRepository,
    IPaperQuestionRuleRepository paperQuestionRuleRepository,
    IUserExamRepository userExamRepository,
    ILocalEventBus eventBus,
    IUserExamQuestionRepository userExamQuestionRepository,
    IPaperSectionRepository paperSectionRepository)
    : DomainService
{
    public async Task<UserExam> CreateAsync(Guid examId, Guid userId)
    {
        await CheckUnfinishedAsync(userId);
        await examManager.CheckCreateUserExamAsync(examId);

        return new UserExam(GuidGenerator.Create(), examId, userId);
    }

    /// <summary>
    /// 检查是否存在未完成的考试
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <returns></returns>
    /// <exception cref="BusinessException"></exception>
    private async Task CheckUnfinishedAsync(Guid userId)
    {
        if (await userExamRepository.UnfinishedExistsAsync(userId))
        {
            throw new BusinessException(ExamDomainErrorCodes.UserExams.UnfinishedAlreadyExists);
        }
    }

    public void Start(UserExam userExam)
    {
        if (userExam.Status != UserExamStatus.Waiting)
        {
            throw new InvalidUserExamStatusException(userExam.Status);
        }
        userExam.Status = UserExamStatus.InProgress;
        userExam.StartTime = Clock.Now;
    }

    /// <summary>
    /// 抽题
    /// </summary>
    /// <param name="userExamId"></param>
    /// <returns></returns>
    public async Task CreateQuestionsAsync(Guid userExamId)
    {
        UserExam userExam = await userExamRepository.GetAsync(userExamId);
        Examination exam = await examRepository.GetAsync(userExam.ExamId);
        Paper paper = await paperRepository.GetAsync(exam.PaperId);

        List<PaperSection> sections = await paperSectionRepository.GetListByPaperIdAsync(paper.Id);
        int sectionIndex = 0;
        int totalSections = sections.Count;

        foreach (var section in sections)
        {
            sectionIndex++;

            if (paper.PaperType == PaperType.Fixed)
            {
                await CreateFixedSectionAsync(userExam, section, sectionIndex, totalSections);
            }
            else if (paper.PaperType == PaperType.Random)
            {
                await CreateRandomSectionAsync(userExam, section, sectionIndex, totalSections);
            }
        }

        Start(userExam);
        await eventBus.PublishAsync(new DataGenerationProgressUpdatedEto
        {
            Progress = 100,
            UserId = userExam.UserId,
        });
    }

    private async Task CreateFixedSectionAsync(UserExam userExam, PaperSection section, int sectionIndex, int totalSections)
    {
        var userExamSection = new UserExamSection(
            GuidGenerator.Create(),
            userExam.Id,
            section.Id,
            section.Title,
            section.ScoreEach,
            section.TotalScore,
            section.Order,
            section.TotalCount);
        userExamSection.TenantId = userExam.TenantId;

        await eventBus.PublishAsync(new DataGenerationProgressUpdatedEto
        {
            Progress = (sectionIndex - 1) / totalSections * 10,
            UserId = userExam.UserId,
        });

        var questions = new List<UserExamQuestion>();
        foreach (var paperQuestion in section.PaperQuestions)
        {
            var userExamQuestion = new UserExamQuestion(
                GuidGenerator.Create(),
                userExamSection.Id,
                paperQuestion.QuestionId,
                paperQuestion.Score,
                paperQuestion.Order);
            userExamQuestion.TenantId = userExam.TenantId;
            questions.Add(userExamQuestion);
        }
        userExamSection.SetQuestions(questions);
        userExam.AddSection(userExamSection);

        await eventBus.PublishAsync(new DataGenerationProgressUpdatedEto
        {
            Progress = sectionIndex / totalSections * 80 + 10,
            UserId = userExam.UserId,
        });
    }

    private async Task CreateRandomSectionAsync(UserExam userExam, PaperSection section, int sectionIndex, int totalSections)
    {
        var userExamSection = new UserExamSection(
            GuidGenerator.Create(),
            userExam.Id,
            section.Id,
            section.Title,
            section.ScoreEach,
            section.TotalScore,
            section.Order,
            section.TotalCount);
        userExamSection.TenantId = userExam.TenantId;

        await eventBus.PublishAsync(new DataGenerationProgressUpdatedEto
        {
            Progress = (sectionIndex - 1) / totalSections * 10,
            UserId = userExam.UserId,
        });

        var questions = new List<UserExamQuestion>();

        foreach (var paperRule in section.PaperQuestionRules)
        {
            List<Question> randomQuestions = await GetRandomQuestions(paperRule.QuestionBankId, paperRule.QuestionType, paperRule.Count);
            foreach (var question in randomQuestions)
            {
                var userExamQuestion = new UserExamQuestion(
                    GuidGenerator.Create(),
                    userExamSection.Id,
                    question.Id,
                    paperRule.Score,
                    0);
                userExamQuestion.TenantId = userExam.TenantId;
                questions.Add(userExamQuestion);
            }
        }
        userExamSection.SetQuestions(questions);
        userExam.AddSection(userExamSection);

        await eventBus.PublishAsync(new DataGenerationProgressUpdatedEto
        {
            Progress = sectionIndex / totalSections * 80 + 10,
            UserId = userExam.UserId,
        });
    }

    private async Task<List<Question>> GetRandomQuestions(Guid questionRepositoryId, QuestionType questionType, int count)
    {
        return await questionRepository.GetRandomListAsync(questionRepositoryId: questionRepositoryId,
            questionType: questionType, maxResultCount: count);
    }

    /// <summary>
    /// 提交实践
    /// </summary>
    /// <param name="examId"></param>
    /// <returns></returns>
    public async Task SubmitUserExamAsync(Guid examId)
    {
        List<UserExam> userExams = await userExamRepository.GetInProgressAsync(examId);
        foreach (var userExam in userExams)
        {
            await eventBus.PublishAsync(new UserExamSubmittedEto
            {
                UserId = userExam.UserId,
            });
        }
    }
}