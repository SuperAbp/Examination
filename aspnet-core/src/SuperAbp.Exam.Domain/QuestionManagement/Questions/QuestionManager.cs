using System;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace SuperAbp.Exam.QuestionManagement.Questions;

public class QuestionManager : DomainService
{
    public QuestionManager(IQuestionRepository questionRepository)
    {
        QuestionRepository = questionRepository;
    }

    protected IQuestionRepository QuestionRepository { get; }

    public virtual async Task<Question> CreateAsync(Guid id, Guid questionRepositoryId, QuestionType questionType, string content)
    {
        await CheckContentAsync(content);

        return new Question(GuidGenerator.Create(), questionRepositoryId, questionType, content);
    }

    protected virtual async Task CheckContentAsync(string content)
    {
        if (await QuestionRepository.ContentExistsAsync(content))
        {
            throw new QuestionContentAlreadyExistException(content);
        }
    }
}