using Volo.Abp;

namespace SuperAbp.Exam.QuestionManagement.Questions.QuestionAnswers;

public class QuestionAnswerCorrectCountErrorException()
    : BusinessException(code: ExamDomainErrorCodes.Questions.CorrectCountError);