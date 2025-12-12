using SuperAbp.Exam.Localization;
using Volo.Abp.Application.Services;

namespace SuperAbp.Exam;

public abstract class ExamAppServiceBase : ApplicationService
{
    protected ExamAppServiceBase()
    {
        LocalizationResource = typeof(ExamResource);
    }
}