using Volo.Abp;

namespace SuperAbp.Exam.ExamManagement.UserExams;

public class UnfinishedException() : BusinessException(code: ExamDomainErrorCodes.UserExams.Unfinished);