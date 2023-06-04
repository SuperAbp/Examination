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

public class ExaminationPaperAppService : ExamAppService, IExaminationPaperAppService
{
    private readonly IExamingRepository _examingRepository;
    private readonly IPaperRepository _paperRepository;
    private readonly IPaperRepoRepository _paperRepoRepository;
    private readonly IQuestionRepository _questionRepository;

    public ExaminationPaperAppService(IExamingRepository examingRepository, IPaperRepository paperRepository, IPaperRepoRepository paperRepoRepository, IQuestionRepository questionRepository)
    {
        _examingRepository = examingRepository;
        _paperRepository = paperRepository;
        _paperRepoRepository = paperRepoRepository;
        _questionRepository = questionRepository;
    }

    public async Task CreateAsync(Guid examingId)
    {
        var examing = await _examingRepository.GetAsync(examingId);
        if (examing.StartTime.HasValue && examing.EndTime.HasValue &&
            (DateTime.Now < examing.StartTime.Value || DateTime.Now > examing.EndTime.Value))
        {
            throw new UserFriendlyException("目前不在考试时间内");
        }

        // TODO:根据考试规则生成用户专属试卷插入UserPaper表？
        // 或者创建考试的时候生成题目插入一张表，用户开始考试直接读取？
        var paper = await _paperRepository.GetAsync(examing.PaperId);
        var paperRepos = await _paperRepoRepository.GetListAsync(examing.PaperId);
        foreach (var paperRepo in paperRepos)
        {
            // TODO:过滤题数
            var questions = await _questionRepository.GetListAsync(paperRepo.QuestionRepositoryId);
        }
    }
}