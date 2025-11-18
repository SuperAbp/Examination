using SuperAbp.Exam.Localization;
using Volo.Abp.AspNetCore.Components;

namespace MyCompanyName.MyProjectName.Blazor.WebApp.Tiered;

public abstract class ExamComponentBase : AbpComponentBase
{
    protected ExamComponentBase()
    {
        LocalizationResource = typeof(ExamResource);
    }
}