using Volo.Abp;

namespace SuperAbp.Exam.ExamManagement.UserExams;

public class UnfinishedAlreadyExistException()
    : BusinessException(code: ExamDomainErrorCodes.UserExams.UnfinishedAlreadyExists);