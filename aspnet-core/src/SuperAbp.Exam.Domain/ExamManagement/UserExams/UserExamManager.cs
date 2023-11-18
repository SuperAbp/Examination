using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SuperAbp.Exam.ExamManagement.Exams;
using SuperAbp.Exam.ExamManagement.UserExamQuestions;
using SuperAbp.Exam.PaperManagement.PaperRepos;
using SuperAbp.Exam.PaperManagement.Papers;
using SuperAbp.Exam.QuestionManagement.Questions;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace SuperAbp.Exam.ExamManagement.UserExams;

public class UserExamManager : DomainService
{
    private readonly IExamRepository _examRepository;
    private readonly IPaperRepository _paperRepository;
    private readonly IPaperRepoRepository _paperRepoRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IUserExamQuestionRepository _userExamQuestionRepository;

    public UserExamManager(IExamRepository examRepository,
        IQuestionRepository questionRepository,
        IPaperRepository paperRepository,
        IPaperRepoRepository paperRepoRepository, IUserExamQuestionRepository userExamQuestionRepository)
    {
        _examRepository = examRepository;
        _questionRepository = questionRepository;
        _paperRepository = paperRepository;
        _paperRepoRepository = paperRepoRepository;
        _userExamQuestionRepository = userExamQuestionRepository;
    }

    /// <summary>
    /// 抽题
    /// </summary>
    /// <param name="userExamId"></param>
    /// <param name="examId"></param>
    /// <returns></returns>
    public async Task CreateQuestionsAsync(Guid userExamId, Guid examId)
    {
        var exam = await _examRepository.FindAsync(examId)
                   ?? throw new UserFriendlyException("题库不存在");
        var paper = await _paperRepository.FindAsync(exam.PaperId)
                    ?? throw new UserFriendlyException("试卷不存在");
        var paperRepos = await _paperRepoRepository.GetListAsync(paperId: paper.Id);
        var examQuestions = new List<UserExamQuestion>();
        foreach (var paperRepo in paperRepos)
        {
            if (paperRepo.SingleCount is > 0)
            {
                var questions = await GetRandomQuestions(paperRepo.QuestionRepositoryId, QuestionType.SingleSelect, paperRepo.SingleCount.Value);
                examQuestions.AddRange(questions.Select(q => new UserExamQuestion(GuidGenerator.Create(), userExamId, q.Id, paperRepo.SingleScore ?? 0)));
            }
            if (paperRepo.MultiCount is > 0)
            {
                var questions = await GetRandomQuestions(paperRepo.QuestionRepositoryId, QuestionType.MultiSelect, paperRepo.MultiCount.Value);
                examQuestions.AddRange(questions.Select(q => new UserExamQuestion(GuidGenerator.Create(), userExamId, q.Id, paperRepo.MultiScore ?? 0)));
            }
            if (paperRepo.JudgeCount is > 0)
            {
                var questions = await GetRandomQuestions(paperRepo.QuestionRepositoryId, QuestionType.Judge, paperRepo.JudgeCount.Value);
                examQuestions.AddRange(questions.Select(q => new UserExamQuestion(GuidGenerator.Create(), userExamId, q.Id, paperRepo.JudgeScore ?? 0)));
            }
            if (paperRepo.BlankCount is > 0)
            {
                var questions = await GetRandomQuestions(paperRepo.QuestionRepositoryId, QuestionType.FillInTheBlanks, paperRepo.BlankCount.Value);
                examQuestions.AddRange(questions.Select(q => new UserExamQuestion(GuidGenerator.Create(), userExamId, q.Id, paperRepo.BlankScore ?? 0)));
            }
        }

        await _userExamQuestionRepository.InsertManyAsync(examQuestions);

        async Task<List<Question>> GetRandomQuestions(Guid questionRepositoryId, QuestionType questionType, int count)
        {
            return await _questionRepository.GetRandomListAsync(questionRepositoryId: questionRepositoryId,
                questionType: questionType, maxResultCount: count);
        }
    }
}