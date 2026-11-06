using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace SuperAbp.Exam.Options;

public interface IOptionAppService : IApplicationService
{
    public Task<Dictionary<int, string>> GetExaminationStatus();

    public Task<Dictionary<int, string>> GetQuestionTypes();

    public Task<Dictionary<int, string>> GetAnswerModes();

    public Task<Dictionary<int, string>> GetReviewModes();

    public Task<Dictionary<int, string>> GetUserExamStatus();
}
