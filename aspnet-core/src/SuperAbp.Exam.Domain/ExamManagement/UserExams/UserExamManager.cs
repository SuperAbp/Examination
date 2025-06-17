using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SuperAbp.Exam.ExamManagement.Exams;
using SuperAbp.Exam.ExamManagement.UserExamQuestions;
using SuperAbp.Exam.PaperManagement.PaperQuestionRules;
using SuperAbp.Exam.PaperManagement.Papers;
using SuperAbp.Exam.QuestionManagement.Questions;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Users;

namespace SuperAbp.Exam.ExamManagement.UserExams;

public class UserExamManager(
    ExamManager examManager,
    IExamRepository examRepository,
    IQuestionRepository questionRepository,
    IPaperRepository paperRepository,
    IPaperQuestionRuleRepository paperQuestionRuleRepository,
    IUserExamRepository userExamRepository,
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
    /// <param name="examId"></param>
    /// <returns></returns>
    public async Task CreateQuestionsAsync(Guid userExamId, Guid examId)
    {
        Examination exam = await examRepository.GetAsync(examId);
        Paper paper = await paperRepository.GetAsync(exam.PaperId);
        List<PaperQuestionRule> paperRepos = await paperQuestionRuleRepository.GetListAsync(paperId: paper.Id);
        List<UserExamQuestion> examQuestions = [];
        foreach (var paperRepo in paperRepos)
        {
            if (paperRepo.SingleCount is > 0)
            {
                var questions = await GetRandomQuestions(paperRepo.QuestionBankId, QuestionType.SingleSelect, paperRepo.SingleCount.Value);
                examQuestions.AddRange(questions.Select(q => new UserExamQuestion(GuidGenerator.Create(), userExamId, q.Id, paperRepo.SingleScore ?? 0)));
            }
            if (paperRepo.MultiCount is > 0)
            {
                var questions = await GetRandomQuestions(paperRepo.QuestionBankId, QuestionType.MultiSelect, paperRepo.MultiCount.Value);
                examQuestions.AddRange(questions.Select(q => new UserExamQuestion(GuidGenerator.Create(), userExamId, q.Id, paperRepo.MultiScore ?? 0)));
            }
            if (paperRepo.JudgeCount is > 0)
            {
                var questions = await GetRandomQuestions(paperRepo.QuestionBankId, QuestionType.Judge, paperRepo.JudgeCount.Value);
                examQuestions.AddRange(questions.Select(q => new UserExamQuestion(GuidGenerator.Create(), userExamId, q.Id, paperRepo.JudgeScore ?? 0)));
            }
            if (paperRepo.BlankCount is > 0)
            {
                var questions = await GetRandomQuestions(paperRepo.QuestionBankId, QuestionType.FillInTheBlanks, paperRepo.BlankCount.Value);
                examQuestions.AddRange(questions.Select(q => new UserExamQuestion(GuidGenerator.Create(), userExamId, q.Id, paperRepo.BlankScore ?? 0)));
            }
        }

        await userExamQuestionRepository.InsertManyAsync(examQuestions);

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