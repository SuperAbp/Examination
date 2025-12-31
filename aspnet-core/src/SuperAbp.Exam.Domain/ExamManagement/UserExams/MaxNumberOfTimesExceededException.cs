using Volo.Abp;

namespace SuperAbp.Exam.ExamManagement.UserExams;

public class MaxNumberOfTimesExceededException : BusinessException
{
    public MaxNumberOfTimesExceededException(int maxNumberOfTimes) : base(code: ExamDomainErrorCodes.UserExams.MaxNumberOfTimesExceeded)
    {
        WithData("MaxNumberOfTimes", maxNumberOfTimes);
    }
}