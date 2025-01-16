using System;
using Volo.Abp;

namespace SuperAbp.Exam.QuestionManagement.Questions;

public class QuestionContentAlreadyExistException : BusinessException
{
    public QuestionContentAlreadyExistException(string content)
        : base(code: ExamDomainErrorCodes.Questions.ContentAlreadyExists)
    {
        WithData(nameof(Question.Content), content);
    }
}