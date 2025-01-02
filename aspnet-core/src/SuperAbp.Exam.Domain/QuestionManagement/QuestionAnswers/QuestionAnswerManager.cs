using System.Threading.Tasks;
using System;
using Volo.Abp.Domain.Services;

namespace SuperAbp.Exam.QuestionManagement.QuestionAnswers;

public class QuestionAnswerManager(IQuestionAnswerRepository questionAnswerRepository) : DomainService
{
    protected IQuestionAnswerRepository QuestionAnswerRepository { get; } = questionAnswerRepository;

    public virtual async Task<QuestionAnswer> CreateAsync(Guid questionId, string content, bool right)
    {
        await CheckContentAsync(questionId, content);

        return new QuestionAnswer(GuidGenerator.Create(), questionId, content, right);
    }

    public virtual async Task SetContentAsync(QuestionAnswer answer, string content)
    {
        if (content == answer.Content)
        {
            return;
        }
        await CheckContentAsync(answer.QuestionId, content);

        answer.Content = content;
    }

    protected virtual async Task CheckContentAsync(Guid questionId, string content)
    {
        if (await QuestionAnswerRepository.ContentExistsAsync(questionId, content))
        {
            throw new QuestionAnswerContentAlreadyExistException(content);
        }
    }
}