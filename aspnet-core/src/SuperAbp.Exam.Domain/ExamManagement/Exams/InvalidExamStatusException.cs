using Volo.Abp;

namespace SuperAbp.Exam.ExamManagement.Exams;

public class InvalidExamStatusException(ExaminationStatus status) : BusinessException(code: ExamDomainErrorCodes.Exams.InvalidStatus);