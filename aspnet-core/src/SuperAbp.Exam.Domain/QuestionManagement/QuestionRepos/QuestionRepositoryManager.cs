using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace SuperAbp.Exam.QuestionManagement.QuestionRepos;

public class QuestionRepositoryManager(IQuestionRepoRepository questionRepoRepository) : DomainService
{
    protected IQuestionRepoRepository QuestionRepoRepository { get; } = questionRepoRepository;

    public virtual async Task<QuestionRepo> CreateAsync(string title)
    {
        await CheckTitleAsync(title);

        return new QuestionRepo(GuidGenerator.Create(), title);
    }

    /// <summary>
    /// 设置标题
    /// </summary>
    /// <param name="repo"></param>
    /// <param name="title">标题</param>
    /// <exception cref="QuestionRepositoryTitleAlreadyExistException">标题已存在</exception>
    /// <returns></returns>
    public virtual async Task SetTitleAsync(QuestionRepo repo, string title)
    {
        if (title == repo.Title)
        {
            return;
        }
        await CheckTitleAsync(title);

        repo.Title = title;
    }

    protected virtual async Task CheckTitleAsync(string title)
    {
        if (await QuestionRepoRepository.TitleExistsAsync(title))
        {
            throw new QuestionRepositoryTitleAlreadyExistException(title);
        }
    }
}