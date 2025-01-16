using Volo.Abp;

namespace SuperAbp.Exam.QuestionManagement.QuestionRepos;

public class QuestionRepositoryTitleAlreadyExistException : BusinessException
{
    public QuestionRepositoryTitleAlreadyExistException(string title)
        : base(code: ExamDomainErrorCodes.QuestionRepositories.TitleAlreadyExists)
    {
        WithData(nameof(QuestionRepo.Title), title);
    }
}