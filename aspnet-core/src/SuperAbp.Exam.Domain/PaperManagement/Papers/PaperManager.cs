using SuperAbp.Exam.ExamManagement.Exams;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace SuperAbp.Exam.PaperManagement.Papers;

public class PaperManager(IPaperRepository paperRepository,
    IExamRepository examRepository) : DomainService
{
    protected IPaperRepository PaperRepository { get; } = paperRepository;
    protected IExamRepository ExamRepository { get; } = examRepository;

    public virtual async Task<Paper> CreateAsync(PaperType paperType, string name, bool manualReview)
    {
        await CheckNameAsync(name);
        return new Paper(GuidGenerator.Create(), paperType, name, manualReview);
    }

    public virtual async Task SetNameAsync(Paper question, string name)
    {
        if (name == question.Name)
        {
            return;
        }
        await CheckNameAsync(name);

        question.Name = name;
    }

    protected virtual async Task CheckNameAsync(string name)
    {
        if (await PaperRepository.NameExistsAsync(name))
        {
            throw new PaperNameAlreadyExistException(name);
        }
    }

    public virtual async Task DeleteAsync(Paper paper)
    {
        if (await examRepository.ExistsByPaperIdAsync(paper.Id))
        {
            throw new PaperUsedByExamException();
        }
        paper.PaperSections.Clear();
        await PaperRepository.DeleteAsync(paper);
    }
}