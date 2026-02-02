using SuperAbp.Exam.QuestionManagement.Questions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace SuperAbp.Exam.QuestionManagement.QuestionBanks;

public class QuestionBankManager(IQuestionBankRepository questionBankRepository,
    IQuestionRepository questionRepository,
    QuestionManager questionManager) : DomainService
{
    protected IQuestionBankRepository QuestionBankRepository { get; } = questionBankRepository;
    protected IQuestionRepository QuestionRepository { get; } = questionRepository;
    protected QuestionManager QuestionManager { get; } = questionManager;

    public virtual async Task<QuestionBank> CreateAsync(string title)
    {
        await CheckTitleAsync(title);

        return new QuestionBank(GuidGenerator.Create(), title);
    }

    /// <summary>
    /// 设置标题
    /// </summary>
    /// <param name="questionBank">题库</param>
    /// <param name="title">标题</param>
    /// <exception cref="QuestionBankTitleAlreadyExistException">标题已存在</exception>
    /// <returns></returns>
    public virtual async Task SetTitleAsync(QuestionBank questionBank, string title)
    {
        if (title == questionBank.Title)
        {
            return;
        }
        await CheckTitleAsync(title);

        questionBank.Title = title;
    }

    protected virtual async Task CheckTitleAsync(string title)
    {
        if (await QuestionBankRepository.TitleExistsAsync(title))
        {
            throw new QuestionBankTitleAlreadyExistException(title);
        }
    }

    public virtual async Task DeleteAsync(QuestionBank questionBank)
    {
        List<Question> questions = await QuestionRepository.GetListAsync(q => q.QuestionBankId == questionBank.Id);
        foreach (var question in questions)
        {
            await QuestionManager.DeleteAsync(question);
        }
        await QuestionBankRepository.DeleteAsync(questionBank);
    }
}