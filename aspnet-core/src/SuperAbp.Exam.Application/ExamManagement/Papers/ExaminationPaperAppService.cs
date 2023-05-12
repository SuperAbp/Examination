using System;
using System.Threading.Tasks;
using Polly;
using SuperAbp.Exam.ExamManagement.Exams;
using SuperAbp.Exam.PaperManagement.PaperRepos;
using SuperAbp.Exam.PaperManagement.Papers;
using Volo.Abp;

namespace SuperAbp.Exam.ExamManagement.Papers;

public class ExaminationPaperAppService : ExamAppService, IExaminationPaperAppService
{
    private readonly IExamingRepository _examingRepository;
    private readonly IPaperRepository _paperRepository;
    private readonly IPaperRepoRepository _paperRepoRepository;

    public ExaminationPaperAppService(IExamingRepository examingRepository, IPaperRepository paperRepository, IPaperRepoRepository paperRepoRepository)
    {
        _examingRepository = examingRepository;
        _paperRepository = paperRepository;
        _paperRepoRepository = paperRepoRepository;
    }

    public async Task CreateAsync(Guid examingId)
    {
        var examing = await _examingRepository.GetAsync(examingId);
        if (examing.StartTime.HasValue && examing.EndTime.HasValue &&
            (DateTime.Now < examing.StartTime.Value || DateTime.Now > examing.EndTime.Value))
        {
            throw new UserFriendlyException("目前不在考试时间内");
        }

        var paper = await _paperRepository.GetAsync(examing.PaperId);
    }
}