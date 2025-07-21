using Volo.Abp;

namespace SuperAbp.Exam.ExamManagement.UserExams;

public class InvalidUserExamStatusException : BusinessException
{
    public InvalidUserExamStatusException(UserExamStatus status) : base(code: ExamDomainErrorCodes.UserExams.InvalidStatus)
    {
        WithData(nameof(UserExam.Status), status);
    }
}