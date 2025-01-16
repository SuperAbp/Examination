using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace SuperAbp.Exam.PaperManagement.Papers;

public class PaperManager(IPaperRepository paperRepository) : DomainService
{
    protected IPaperRepository PaperRepository { get; } = paperRepository;

    public virtual async Task<Paper> CreateAsync(string name, decimal score)
    {
        await CheckNameAsync(name);
        return new Paper(GuidGenerator.Create(), name, score);
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
}