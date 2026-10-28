using SuperAbp.Exam.Localization;
using Volo.Abp.AspNetCore.Components;

namespace SuperAbp.Exam.Admin.Blazor.Client;

public abstract class ExamComponentBase : AbpComponentBase
{
    protected ExamComponentBase()
    {
        LocalizationResource = typeof(ExamResource);
    }
}