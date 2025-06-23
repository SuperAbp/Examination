using Volo.Abp;

namespace SuperAbp.Exam.ExamManagement.UserExams;

public class InvalidUserExamStatusException(UserExamStatus status) : BusinessException(code: ExamDomainErrorCodes.UserExams.InvalidStatus);