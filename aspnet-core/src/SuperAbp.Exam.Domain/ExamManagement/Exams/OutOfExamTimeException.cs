using Volo.Abp;

namespace SuperAbp.Exam.ExamManagement.Exams;

public class OutOfExamTimeException()
    : BusinessException(code: ExamDomainErrorCodes.Exams.OutOfExamTime);