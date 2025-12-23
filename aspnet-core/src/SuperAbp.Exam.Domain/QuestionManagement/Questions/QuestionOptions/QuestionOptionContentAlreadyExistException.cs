using Volo.Abp;

namespace SuperAbp.Exam.QuestionManagement.Questions.QuestionOptions;

public class QuestionOptionContentAlreadyExistException : BusinessException
{
    public QuestionOptionContentAlreadyExistException(string content)
        : base(code: ExamDomainErrorCodes.QuestionOptions.ContentAlreadyExists)
    {
        WithData(nameof(QuestionOption.Content), content);
    }
}