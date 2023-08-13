using System;
using System.Threading.Tasks;
using Polly;
using SuperAbp.Exam.ExamManagement.Exams;
using SuperAbp.Exam.PaperManagement.PaperRepos;
using SuperAbp.Exam.PaperManagement.Papers;
using SuperAbp.Exam.QuestionManagement.QuestionRepos;
using SuperAbp.Exam.QuestionManagement.Questions;
using Volo.Abp;

namespace SuperAbp.Exam.ExamManagement.Papers;

public class ExamPaperAppService : ExamAppService, IExamPaperAppService
{
    private readonly IExamRepository _examRepository;
    private readonly IPaperRepository _paperRepository;
    private readonly IPaperRepoRepository _paperRepoRepository;
    private readonly IQuestionRepository _questionRepository;

    public ExamPaperAppService(IExamRepository examRepository, IPaperRepository paperRepository, IPaperRepoRepository paperRepoRepository, IQuestionRepository questionRepository)
    {
        _examRepository = examRepository;
        _paperRepository = paperRepository;
        _paperRepoRepository = paperRepoRepository;
        _questionRepository = questionRepository;
    }

    public async Task CreateAsync(Guid examId)
    {
        var examination = await _examRepository.GetAsync(examId);
        if (examination.StartTime.HasValue && examination.EndTime.HasValue &&
            (DateTime.Now < examination.StartTime.Value || DateTime.Now > examination.EndTime.Value))
        {
            throw new UserFriendlyException("目前不在考试时间内");
        }

        // TODO:根据考试规则生成用户专属试卷插入UserPaper表？
        // 或者创建考试的时候生成题目插入一张表，用户开始考试直接读取？
        var paper = await _paperRepository.GetAsync(examination.PaperId);
        var paperRepos = await _paperRepoRepository.GetListAsync(examination.PaperId);
        foreach (var paperRepo in paperRepos)
        {
            // TODO:过滤题数
            var questions = await _questionRepository.GetListAsync(paperRepo.QuestionRepositoryId);
        }
    }
}