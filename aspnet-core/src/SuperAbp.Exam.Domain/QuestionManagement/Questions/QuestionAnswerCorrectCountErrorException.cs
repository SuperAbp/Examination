using Volo.Abp;

namespace SuperAbp.Exam.QuestionManagement.Questions;

public class QuestionAnswerCorrectCountErrorException()
    : BusinessException(code: ExamDomainErrorCodes.Questions.CorrectCountError);