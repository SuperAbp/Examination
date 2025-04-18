using System.Collections.Generic;
using Volo.Abp.Application.Services;

namespace SuperAbp.Exam.Admin.Options;

public interface IOptionAppService : IApplicationService
{
    public Dictionary<int, string> GetQuestionTypes();
}