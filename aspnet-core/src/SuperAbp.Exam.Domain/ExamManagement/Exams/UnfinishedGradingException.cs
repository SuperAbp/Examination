using Volo.Abp;

namespace SuperAbp.Exam.ExamManagement.Exams;

public class UnfinishedGradingException : BusinessException
{
    public UnfinishedGradingException() : base(code: ExamDomainErrorCodes.Exams.UnfinishedGrading)
    {
    }
}