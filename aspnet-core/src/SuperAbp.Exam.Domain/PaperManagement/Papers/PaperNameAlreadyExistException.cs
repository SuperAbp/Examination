using SuperAbp.Exam.QuestionManagement.Questions;
using Volo.Abp;

namespace SuperAbp.Exam.PaperManagement.Papers;

public class PaperNameAlreadyExistException : BusinessException
{
    public PaperNameAlreadyExistException(string name) : base(code: ExamDomainErrorCodes.Papers.NameAlreadyExists)
    {
        WithData(nameof(Paper.Name), name);
    }
}