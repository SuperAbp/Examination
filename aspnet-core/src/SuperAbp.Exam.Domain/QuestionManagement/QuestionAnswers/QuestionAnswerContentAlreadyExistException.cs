using Volo.Abp;

namespace SuperAbp.Exam.QuestionManagement.QuestionAnswers;

public class QuestionAnswerContentAlreadyExistException : BusinessException
{
    public QuestionAnswerContentAlreadyExistException(string content)
        : base(code: ExamDomainErrorCodes.QuestionAnswers.ContentAlreadyExists)
    {
        WithData(nameof(QuestionAnswer.Content), content);
    }
}