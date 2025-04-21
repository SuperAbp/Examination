using Volo.Abp;

namespace SuperAbp.Exam.QuestionManagement.QuestionBanks;

public class QuestionBankTitleAlreadyExistException : BusinessException
{
    public QuestionBankTitleAlreadyExistException(string title)
        : base(code: ExamDomainErrorCodes.QuestionBanks.TitleAlreadyExists)
    {
        WithData(nameof(QuestionBank.Title), title);
    }
}