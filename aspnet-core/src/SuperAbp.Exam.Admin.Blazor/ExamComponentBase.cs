using SuperAbp.Exam.Localization;
using Volo.Abp.AspNetCore.Components;

namespace SuperAbp.Exam.Blazor;

public abstract class ExamComponentBase : AbpComponentBase
{
    protected ExamComponentBase()
    {
        LocalizationResource = typeof(ExamResource);
    }
}
