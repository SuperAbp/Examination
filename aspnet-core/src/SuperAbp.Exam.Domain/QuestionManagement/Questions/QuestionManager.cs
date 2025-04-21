using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace SuperAbp.Exam.QuestionManagement.Questions;

public class QuestionManager(IQuestionRepository questionRepository) : DomainService
{
    protected IQuestionRepository QuestionRepository { get; } = questionRepository;

    public virtual async Task<Question> CreateAsync(Guid questionRepositoryId, Guid questionCategoryId, QuestionType questionType, string content)
    {
        await CheckContentAsync(content);

        return new Question(GuidGenerator.Create(), questionRepositoryId, questionCategoryId, questionType, content);
    }

    public virtual async Task SetContentAsync(Question question, string content)
    {
        if (content == question.Content)
        {
            return;
        }
        await CheckContentAsync(content);

        question.Content = content;
    }

    protected virtual async Task CheckContentAsync(string content)
    {
        if (await QuestionRepository.ContentExistsAsync(content))
        {
            throw new QuestionContentAlreadyExistException(content);
        }
    }
}