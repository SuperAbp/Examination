using Volo.Abp;

namespace SuperAbp.Exam.ExamManagement.UserExams;

public class OutOfExamTimeException()
    : BusinessException(code: ExamDomainErrorCodes.Exams.OutOfExamTime);