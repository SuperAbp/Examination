using System.Collections.Generic;
using Volo.Abp.Application.Services;

namespace SuperAbp.Exam.Options;

public interface IOptionAppService : IApplicationService
{
    public Dictionary<int, string> GetExaminationStatus();

    public Dictionary<int, string> GetQuestionTypes();

    public Dictionary<int, string> GetAnswerModes();

    public Dictionary<int, string> GetReviewModes();

    public Dictionary<int, string> GetUserExamStatus();
}