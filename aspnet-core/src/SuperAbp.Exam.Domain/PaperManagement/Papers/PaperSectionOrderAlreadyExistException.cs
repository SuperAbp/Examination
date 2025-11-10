using SuperAbp.Exam;

namespace SuperAbp.Exam.PaperManagement.Papers;

public class PaperSectionOrderAlreadyExistException : Volo.Abp.BusinessException
{
    public PaperSectionOrderAlreadyExistException(int order)
        : base(ExamDomainErrorCodes.Papers.SectionOrderAlreadyExist)
    {
        WithData("order", order);
    }
}
