using System.Threading.Tasks;
using System;
using Volo.Abp.Domain.Services;

namespace SuperAbp.Exam.QuestionManagement.QuestionAnswers;

public class QuestionAnswerManager(IQuestionAnswerRepository questionAnswerRepository) : DomainService
{
    protected IQuestionAnswerRepository QuestionAnswerRepository { get; } = questionAnswerRepository;

    public virtual async Task<QuestionAnswer> CreateAsync(Guid questionId, string content, bool right)
    {
        await CheckContentAsync(content);

        return new QuestionAnswer(GuidGenerator.Create(), questionId, content, right);
    }

    protected virtual async Task CheckContentAsync(string content)
    {
        if (await QuestionAnswerRepository.ContentExistsAsync(content))
        {
            throw new QuestionAnswerContentAlreadyExistException(content);
        }
    }
}