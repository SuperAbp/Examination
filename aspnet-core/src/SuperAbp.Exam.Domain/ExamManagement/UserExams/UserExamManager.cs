using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SuperAbp.Exam.ExamManagement.Exams;
using SuperAbp.Exam.ExamManagement.UserExamQuestions;
using SuperAbp.Exam.PaperManagement.PaperQuestionRules;
using SuperAbp.Exam.PaperManagement.Papers;
using SuperAbp.Exam.QuestionManagement.Questions;
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
    IUserExamQuestionRepository userExamQuestionRepository)
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
    /// <exception cref="UnfinishedAlreadyExistException"></exception>
    private async Task CheckUnfinishedAsync(Guid userId)
    {
        if (await userExamRepository.UnfinishedExistsAsync(userId))
        {
            throw new UnfinishedAlreadyExistException();
        }
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
        List<PaperQuestionRule> paperRepos = await paperQuestionRuleRepository.GetListAsync(paperId: paper.Id);

        await eventBus.PublishAsync(new DataGenerationProgressUpdatedEto
        {
            Progress = 10,
            UserId = userExam.UserId,
        });

        int i = 0;
        foreach (var paperRepo in paperRepos)
        {
            i++;
            if (paperRepo.SingleCount is > 0)
            {
                List<Question> questions = await GetRandomQuestions(paperRepo.QuestionBankId, QuestionType.SingleSelect, paperRepo.SingleCount.Value);
                userExam.Questions.AddRange(questions.Select(q => new UserExamQuestion(GuidGenerator.Create(), userExam.Id, q.Id, paperRepo.SingleScore ?? 0)));
            }
            if (paperRepo.MultiCount is > 0)
            {
                List<Question> questions = await GetRandomQuestions(paperRepo.QuestionBankId, QuestionType.MultiSelect, paperRepo.MultiCount.Value);
                userExam.Questions.AddRange(questions.Select(q => new UserExamQuestion(GuidGenerator.Create(), userExam.Id, q.Id, paperRepo.MultiScore ?? 0)));
            }
            if (paperRepo.JudgeCount is > 0)
            {
                List<Question> questions = await GetRandomQuestions(paperRepo.QuestionBankId, QuestionType.Judge, paperRepo.JudgeCount.Value);
                userExam.Questions.AddRange(questions.Select(q => new UserExamQuestion(GuidGenerator.Create(), userExam.Id, q.Id, paperRepo.JudgeScore ?? 0)));
            }
            if (paperRepo.BlankCount is > 0)
            {
                List<Question> questions = await GetRandomQuestions(paperRepo.QuestionBankId, QuestionType.FillInTheBlanks, paperRepo.BlankCount.Value);
                userExam.Questions.AddRange(questions.Select(q => new UserExamQuestion(GuidGenerator.Create(), userExam.Id, q.Id, paperRepo.BlankScore ?? 0)));
            }

            await eventBus.PublishAsync(new DataGenerationProgressUpdatedEto
            {
                Progress = i / paperRepos.Count * 80,
                UserId = userExam.UserId,
            });
        }

        // TODO:Save failed;
        await userExamRepository.UpdateAsync(userExam);

        await eventBus.PublishAsync(new DataGenerationProgressUpdatedEto
        {
            Progress = 100,
            UserId = userExam.UserId,
        });

        async Task<List<Question>> GetRandomQuestions(Guid questionRepositoryId, QuestionType questionType, int count)
        {
            return await questionRepository.GetRandomListAsync(questionRepositoryId: questionRepositoryId,
                questionType: questionType, maxResultCount: count);
        }
    }

    public async Task<List<UserExamQuestionWithDetails>> GetQuestionsAsync(Guid userExamId)
    {
        return await userExamQuestionRepository.GetListAsync(userExamId: userExamId);
    }
}