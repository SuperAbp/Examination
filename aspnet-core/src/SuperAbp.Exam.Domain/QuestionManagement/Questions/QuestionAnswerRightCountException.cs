using Volo.Abp;

namespace SuperAbp.Exam.QuestionManagement.Questions;

public class QuestionAnswerRightCountException()
    : BusinessException(code: ExamDomainErrorCodes.Questions.RightCountError);