using Volo.Abp;

namespace SuperAbp.Exam.ExamManagement.Exams;

public class InvalidExamStatusException : BusinessException
{
    public InvalidExamStatusException(ExaminationStatus status) : base(code: ExamDomainErrorCodes.Exams.InvalidStatus)
    {
        WithData(nameof(Examination.Status), status);
    }
}