using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace SuperAbp.Exam.QuestionManagement.QuestionBanks;

public class QuestionBankManager(IQuestionBankRepository questionBankRepository) : DomainService
{
    protected IQuestionBankRepository QuestionBankRepository { get; } = questionBankRepository;

    public virtual async Task<QuestionBank> CreateAsync(string title)
    {
        await CheckTitleAsync(title);

        return new QuestionBank(GuidGenerator.Create(), title);
    }

    /// <summary>
    /// 设置标题
    /// </summary>
    /// <param name="repo"></param>
    /// <param name="title">标题</param>
    /// <exception cref="QuestionBankTitleAlreadyExistException">标题已存在</exception>
    /// <returns></returns>
    public virtual async Task SetTitleAsync(QuestionBank repo, string title)
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
        if (await QuestionBankRepository.TitleExistsAsync(title))
        {
            throw new QuestionBankTitleAlreadyExistException(title);
        }
    }
}